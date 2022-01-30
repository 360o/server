using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using _360o.Server.API.V1.Stores.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Net;

namespace _360o.Server.API.V1.Stores.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public StoresController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> CreateStoreAsync([FromBody] CreateStoreRequest request)
        {
            var validator = new CreateStoreRequestValidator();

            validator.ValidateAndThrow(request);

            var store = new Store(organizationId: request.OrganizationId, place: _mapper.Map<Place>(request.Place));

            _apiContext.Add(store);

            await _apiContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStoreByIdAsync), new { id = store.Id }, _mapper.Map<StoreDTO>(store));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreDTO>> GetStoreByIdAsync(Guid id)
        {
            var store = await _apiContext.Stores.Include(s => s.Place).SingleOrDefaultAsync(m => m.Id == id);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.ItemNotFound.ToString());
            }

            return _mapper.Map<StoreDTO>(store);
        }

        [HttpGet]
        public async Task<IList<StoreDTO>> ListStoresAsync([FromQuery] ListStoresRequest request)
        {
            var validator = new ListStoresRequestValidator();

            validator.ValidateAndThrow(request);

            var stores = _apiContext.Stores.Include(m => m.Place).AsQueryable();

            if (request.Query != null)
            {
                stores = stores.Where(s => s.Organization.EnglishSearchVector.Matches(EF.Functions.WebSearchToTsQuery("english", request.Query)) || s.Organization.FrenchSearchVector.Matches(EF.Functions.WebSearchToTsQuery("french", request.Query)));
            }

            if (request.Latitude.HasValue && request.Longitude.HasValue && request.Radius.HasValue)
            {
                var locationPoint = new Point(x: request.Longitude.Value, y: request.Latitude.Value);
                stores = stores.Where(p => p.Place.Point.Distance(locationPoint) < request.Radius.Value);
            }

            return await stores.Select(m => _mapper.Map<StoreDTO>(m)).ToListAsync();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStoreByIdAsync(Guid id)
        {
            var store = await _apiContext.Stores.FindAsync(id);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.ItemNotFound.ToString());
            }

            _apiContext.Remove(store);

            await _apiContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

