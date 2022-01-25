using _360o.Server.API.V1.Stores.Controllers.DTOs;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Npgsql;

namespace _360o.Server.API.V1.Stores.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly StoresContext _storesContext;
        private readonly IMapper _mapper;

        public StoresController(StoresContext storesContext, IMapper mapper)
        {
            _storesContext = storesContext;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> CreateStoreAsync([FromBody] CreateStoreRequest request)
        {
            var merchant = new Store(userId: User.Identity.Name, displayName: request.DisplayName, englishShortDescription: request.EnglishShortDescription, englishLongDescription: request.EnglishLongDescription, englishCategories: request.EnglishCategories, frenchShortDescription: request.FrenchShortDescription, frenchLongDescription: request.FrenchLongDescription, frenchCategories: request.FrenchCategories, place: _mapper.Map<Place>(request.Place));

            _storesContext.Add(merchant);

            try
            {
                await _storesContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                // See https://www.postgresql.org/docs/current/errcodes-appendix.html
                if (e.InnerException is PostgresException innerException && innerException.SqlState == "23505")
                {
                    return Conflict(new ErrorDTO
                    {
                        Code = ErrorCode.NameAlreadyExists,
                        Message = e.InnerException.Message
                    });
                }

                throw;
            }

            return CreatedAtAction(nameof(GetStoreByIdAsync), new { id = merchant.Id }, _mapper.Map<StoreDTO>(merchant));
        }

        [HttpGet]
        public async Task<IEnumerable<StoreDTO>> ListStoresAsync([FromQuery] ListStoresRequest request)
        {
            var merchants = _storesContext.Merchants.Include(m => m.Place).AsQueryable();

            if (request.Query != null)
            {
                merchants = merchants.Where(m => m.EnglishSearchVector.Matches(request.Query) || m.FrenchSearchVector.Matches(request.Query));
            }

            if (request.Latitude.HasValue && request.Longitude.HasValue && request.Radius.HasValue)
            {
                var locationPoint = new Point(x: request.Longitude.Value, y: request.Latitude.Value);
                merchants = merchants.Where(p => p.Place.Point.Distance(locationPoint) < request.Radius.Value);
            }

            return await merchants.Select(m => _mapper.Map<StoreDTO>(m)).ToListAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreDTO>> GetStoreByIdAsync(Guid id)
        {
            var merchant = await _storesContext.Merchants.Include(s => s.Place).SingleOrDefaultAsync(m => m.Id == id);

            if (merchant == null)
            {
                return NotFound(new ErrorDTO
                {
                    Code = ErrorCode.ItemNotFound,
                    Message = $"Store not found"
                });
            }

            return _mapper.Map<StoreDTO>(merchant);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStoreByIdAsync(Guid id)
        {
            var merchant = await _storesContext.Merchants.FindAsync(id);

            if (merchant == null)
            {
                return NotFound(new ErrorDTO
                {
                    Code = ErrorCode.ItemNotFound,
                    Message = $"Store not found"
                });
            }

            _storesContext.Remove(merchant);

            await _storesContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

