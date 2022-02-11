using _360o.Server.Api.V1.Stores.Model;
using _360o.Server.Api.V1.Stores.Services.Inputs;

namespace _360o.Server.Api.V1.Stores.Services
{
    public interface IStoresService
    {
        Task<Store> CreateStoreAsync(CreateStoreInput input);

        Task<Store?> GetStoreByIdByAsync(Guid storeId);

        Task<Store> UpdateStoreAsync(Store store);

        Task<IList<Store>> ListStoresAsync(ListStoresInput input);

        Task DeleteStoreByIdAsync(Guid storeId);

        Task<Item> CreateItemAsync(CreateItemInput input);

        Task<Item?> GetItembyIdAsync(Guid itemId);

        Task<IList<Item>> ListItemsAsync(Guid storeId);

        Task<Item> UpdateItemAsync(Item item);

        Task DeleteItemByIdAsync(Guid itemId);

        Task<Offer> CreateOfferAsync(Guid storeId, CreateOfferInput input);

        Task<Offer?> GetOfferByIdAsync(Guid offerId);

        Task<IList<Offer>> ListOffersAsync(Guid storeId);

        Task<Offer> UpdateOfferAsync(Offer offer);
    }
}