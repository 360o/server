using _360o.Server.Api.V1.Errors.Enums;
using _360o.Server.Api.V1.Organizations.Services;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using _360o.Server.Api.V1.Stores.Services;
using _360o.Server.Api.V1.Stores.Services.Inputs;
using _360o.Server.Api.V1.Stores.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace _360o.Server.Api.V1.Stores.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IOrganizationsService _organizationsService;
        private readonly IStoresService _storesService;

        public StoresController(IOrganizationsService organizations, IStoresService storesService)
        {
            _organizationsService = organizations;
            _storesService = storesService;
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

            var createStoreInput = new CreateStoreInput
            {
                OrganizationId = organization.Id,
                Place = new Place(
                    request.Place.GooglePlaceId,
                    request.Place.FormattedAddress,
                    new Location(
                        request.Place.Location.Latitude,
                        request.Place.Location.Longitude
                        ))
            };

            var store = await _storesService.CreateStoreAsync(createStoreInput);

            return CreatedAtAction(nameof(GetStoreByIdAsync), new { id = store.Id }, ToStoreDTO(store));
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

            return ToStoreDTO(store);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<StoreDTO>))]
        public async Task<IList<StoreDTO>> ListStoresAsync([FromQuery] ListStoresRequest request)
        {
            var validator = new ListStoresRequestValidator();

            validator.ValidateAndThrow(request);

            var listStoresInput = new ListStoresInput(
                request.Query,
                request.Latitude,
                request.Longitude,
                request.Radius);

            var stores = await _storesService.ListStoresAsync(listStoresInput);

            return stores.Select(s => ToStoreDTO(s)).ToList();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StoreDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<StoreDTO>> PatchStoreAsync(Guid id, JsonPatchDocument<Store> patchDoc)
        {
            if (patchDoc != null)
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

                try
                {
                    patchDoc.ApplyTo(store, ModelState);
                }
                catch (JsonSerializationException ex)
                {
                    if (ex.InnerException is ArgumentException)
                    {
                        return Problem(detail: ex.InnerException.Message, statusCode: (int)HttpStatusCode.BadRequest, title: ErrorCode.InvalidRequest.ToString());
                    }

                    throw;
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                store = await _storesService.UpdateStoreAsync(store);

                return ToStoreDTO(store);
            }
            else
            {
                return BadRequest(ModelState);
            }
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

            var price = request.Price.HasValue ?
                new MoneyValue(request.Price.Value.Amount, request.Price.Value.CurrencyCode) :
                null
                ;

            var createItemInput = new CreateItemInput(
                store.Id,
                request.EnglishName,
                request.EnglishDescription,
                request.FrenchName,
                request.FrenchDescription,
                price
                );

            var item = await _storesService.CreateItemAsync(createItemInput);

            return CreatedAtAction(nameof(GetItemByIdAsync), new { storeId = storeId, itemId = item.Id }, ToItemDTO(item));
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

            return ToItemDTO(item);
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

            return items.Select(i => ToItemDTO(i)).ToList();
        }

        [HttpPatch("{storeId}/items/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<ItemDTO>> PatchItemAsync(Guid storeId, Guid itemId, [FromBody] JsonPatchDocument<Item> patchDoc)
        {
            if (patchDoc != null)
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

                try
                {
                    patchDoc.ApplyTo(item, ModelState);
                }
                catch (JsonSerializationException ex)
                {
                    if (ex.InnerException is ArgumentException)
                    {
                        return Problem(detail: ex.InnerException.Message, statusCode: (int)HttpStatusCode.BadRequest, title: ErrorCode.InvalidRequest.ToString());
                    }

                    throw;
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                item = await _storesService.UpdateItemAsync(item);

                return ToItemDTO(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
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

            var discount = request.Discount.HasValue ?
                new MoneyValue(request.Discount.Value.Amount, request.Discount.Value.CurrencyCode) :
                null
                ;

            var createOfferInput = new CreateOfferInput
            {
                EnglishName = request.EnglishName,
                FrenchName = request.FrenchName,
                OfferItems = request.OfferItems.Select(o => new CreateOfferItem
                {
                    ItemId = o.ItemId,
                    Quantity = o.Quantity,
                }).ToHashSet(),
                Discount = discount,
            };

            var offer = await _storesService.CreateOfferAsync(store.Id, createOfferInput);

            return CreatedAtAction(nameof(GetOfferByIdAsync), new { storeId = store.Id, offerId = offer.Id }, ToOfferDTO(offer));
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

            return ToOfferDTO(offer);
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

            return offers.Select(o => ToOfferDTO(o)).ToList();
        }

        private StoreDTO ToStoreDTO(Store store)
        {
            var place = new PlaceDTO
            {
                GooglePlaceId = store.Place.GooglePlaceId,
                FormattedAddress = store.Place.FormattedAddress,
                Location = new LocationDTO(
                    store.Place.Location.Latitude,
                    store.Place.Location.Longitude
                    )
            };

            return new StoreDTO(store.Id, place, store.OrganizationId);
        }

        private ItemDTO ToItemDTO(Item item)
        {
            MoneyValueDTO? price = item.Price == null ?
                null :
                new MoneyValueDTO(
                    item.Price.Amount,
                    item.Price.CurrencyCode
                    );

            return new ItemDTO(
                item.Id,
                item.EnglishName,
                item.EnglishDescription,
                item.FrenchName,
                item.FrenchDescription,
                price,
                item.StoreId);
        }

        private OfferDTO ToOfferDTO(Offer offer)
        {
            MoneyValueDTO? discount = offer.Discount == null ?
                null :
                new MoneyValueDTO(
                    offer.Discount.Amount,
                    offer.Discount.CurrencyCode
                    );

            return new OfferDTO(
                offer.Id,
                offer.EnglishName,
                offer.FrenchName,
                offer.OfferItems.Select(i => new OfferItemDTO
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                }),
                discount,
                offer.StoreId
                );
        }

        [HttpDelete("{storeId}/offers/{offerId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteOfferByIdAsync(Guid storeId, Guid offerId)
        {
            var offer = await _storesService.GetOfferByIdAsync(offerId);

            if (offer == null)
            {
                return Problem(detail: "Offer not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (offer.StoreId != storeId)
            {
                return Problem(detail: "Store not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != offer.Store.Organization.UserId)
            {
                return Forbid();
            }

            await _storesService.DeleteOfferByIdAsync(offerId);

            return NoContent();
        }
    }
}