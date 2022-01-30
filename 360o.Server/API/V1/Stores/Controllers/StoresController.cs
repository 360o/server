using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using _360o.Server.API.V1.Stores.Services;
using _360o.Server.API.V1.Stores.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _360o.Server.API.V1.Stores.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoresService _storesService;
        private readonly IMapper _mapper;

        public StoresController(IStoresService storesService, IMapper mapper)
        {
            _storesService = storesService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> CreateStoreAsync([FromBody] CreateStoreRequest request)
        {
            var validator = new CreateStoreRequestValidator();

            validator.ValidateAndThrow(request);

            try
            {
                var store = await _storesService.CreateStoreAsync(_mapper.Map<CreateStoreInput>(request));

                return CreatedAtAction(nameof(GetStoreByIdAsync), new { id = store.Id }, _mapper.Map<StoreDTO>(store));

            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.ItemNotFound.ToString());
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreDTO>> GetStoreByIdAsync(Guid id)
        {
            var store = await _storesService.GetStoreByIdByAsync(id);

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

            var stores = await _storesService.ListStoresAsync(_mapper.Map<ListStoresInput>(request));

            return stores.Select(s => _mapper.Map<StoreDTO>(s)).ToList();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStoreByIdAsync(Guid id)
        {
            try
            {
                await _storesService.DeleteStoreByIdAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.ItemNotFound.ToString());
            }

            return NoContent();
        }
    }
}

