using _360o.Server.Api.V1.Errors.Enums;
using _360o.Server.Api.V1.Organizations.Services;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Services;
using _360o.Server.Api.V1.Stores.Services.Inputs;
using _360o.Server.Api.V1.Stores.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _360o.Server.Api.V1.Stores.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IOrganizationsService _organizationsService;
        private readonly IStoresService _storesService;
        private readonly IMapper _mapper;

        public StoresController(IOrganizationsService organizations, IStoresService storesService, IMapper mapper)
        {
            _organizationsService = organizations;
            _storesService = storesService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StoreDTO))]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> CreateStoreAsync([FromBody] CreateStoreRequest request)
        {
            var validator = new CreateStoreRequestValidator();

            validator.ValidateAndThrow(request);

            var organization = await _organizationsService.GetOrganizationByIdAsync(request.OrganizationId);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != organization.UserId)
            {
                return Forbid();
            }

            var store = await _storesService.CreateStoreAsync(_mapper.Map<CreateStoreInput>(request));

            return CreatedAtAction(nameof(GetStoreByIdAsync), new { id = store.Id }, _mapper.Map<StoreDTO>(store));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StoreDTO>> GetStoreByIdAsync(Guid id)
        {
            var store = await _storesService.GetStoreByIdByAsync(id);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            return _mapper.Map<StoreDTO>(store);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<StoreDTO>))]
        public async Task<IList<StoreDTO>> ListStoresAsync([FromQuery] ListStoresRequest request)
        {
            var validator = new ListStoresRequestValidator();

            validator.ValidateAndThrow(request);

            var stores = await _storesService.ListStoresAsync(_mapper.Map<ListStoresInput>(request));

            return stores.Select(s => _mapper.Map<StoreDTO>(s)).ToList();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> UpdateStoreAsync(Guid id, PatchStoreRequest request)
        {
            var store = await _storesService.GetStoreByIdByAsync(id);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != store.Organization.UserId)
            {
                return Forbid();
            }

            store = await _storesService.PatchStoreAsync(store.Id, _mapper.Map<PatchStoreInput>(request));

            return _mapper.Map<StoreDTO>(store);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteStoreByIdAsync(Guid id)
        {
            var store = await _storesService.GetStoreByIdByAsync(id);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != store.Organization.UserId)
            {
                return Forbid();
            }

            await _storesService.DeleteStoreByIdAsync(id);

            return NoContent();
        }

        [HttpPost("{storeId}/items")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ItemDTO))]
        [Authorize]
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(Guid storeId, [FromBody] CreateItemRequest request)
        {
            var validator = new CreateItemRequestValidator();

            validator.ValidateAndThrow(request);

            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            var organization = await _organizationsService.GetOrganizationByIdAsync(store.OrganizationId);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != organization.UserId)
            {
                return Forbid();
            }

            var item = await _storesService.CreateItemAsync(_mapper.Map<CreateItemInput>(request) with { StoreId = storeId });

            return CreatedAtAction(nameof(GetItemByIdAsync), new { storeId = storeId, itemId = item.Id }, _mapper.Map<ItemDTO>(item));
        }

        [HttpGet("{storeId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemDTO>> GetItemByIdAsync(Guid storeId, Guid itemId)
        {
            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            var item = await _storesService.GetItembyIdAsync(itemId);

            if (item == null)
            {
                return Problem(detail: "Item not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            return _mapper.Map<ItemDTO>(item);
        }

        [HttpGet("{storeId}/items")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<ItemDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<ItemDTO>>> ListItemsAsync(Guid storeId)
        {
            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            var items = await _storesService.ListItemsAsync(storeId);

            return items.Select(i => _mapper.Map<ItemDTO>(i)).ToList();
        }

        [HttpPatch("{storeId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<ItemDTO>> UpdateItemAsync(Guid storeId, Guid itemId, [FromBody] PatchItemRequest request)
        {
            var validator = new PatchItemRequestValidator();

            validator.ValidateAndThrow(request);

            var item = await _storesService.GetItembyIdAsync(itemId);

            if (item == null)
            {
                return Problem(detail: "Item not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (item.StoreId != storeId)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != item.Store.Organization.UserId)
            {
                return Forbid();
            }

            item = await _storesService.PatchItemAsync(item.Id, _mapper.Map<PatchItemInput>(request));

            return _mapper.Map<ItemDTO>(item);
        }

        [HttpDelete("{storeId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteItemByIdAsync(Guid storeId, Guid itemId)
        {
            var item = await _storesService.GetItembyIdAsync(itemId);

            if (item == null)
            {
                return Problem(detail: "Item not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (item.StoreId != storeId)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != item.Store.Organization.UserId)
            {
                return Forbid();
            }

            await _storesService.DeleteItemByIdAsync(itemId);

            return NoContent();
        }

        [HttpPost("{storeId}/offers")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OfferDTO))]
        [Authorize]
        public async Task<ActionResult<OfferDTO>> CreateOfferAsync(Guid storeId, [FromBody] CreateOfferRequest request)
        {
            var validator = new CreateOfferRequestValidator();

            validator.ValidateAndThrow(request);

            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != store.Organization.UserId)
            {
                return Forbid();
            }

            var offer = await _storesService.CreateOfferAsync(store.Id, _mapper.Map<CreateOrUpdateOfferInput>(request));

            return CreatedAtAction(nameof(GetOfferByIdAsync), new { storeId = store.Id, offerId = offer.Id }, _mapper.Map<OfferDTO>(offer));
        }

        [HttpGet("{storeId}/offers/{offerId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfferDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OfferDTO>> GetOfferByIdAsync(Guid storeId, Guid offerId)
        {
            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            var offer = await _storesService.GetOfferByIdAsync(offerId);

            if (offer == null)
            {
                return Problem(detail: "Offer not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            return _mapper.Map<OfferDTO>(offer);
        }

        [HttpGet("{storeId}/offers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OfferDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IList<OfferDTO>>> ListOffersAsync(Guid storeId)
        {
            var store = await _storesService.GetStoreByIdByAsync(storeId);

            if (store == null)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            var offers = await _storesService.ListOffersAsync(storeId);

            return offers.Select(o => _mapper.Map<OfferDTO>(o)).ToList();
        }
    }
}