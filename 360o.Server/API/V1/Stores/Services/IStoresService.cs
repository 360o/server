using _360o.Server.API.V1.Stores.Model;
using _360o.Server.API.V1.Stores.Services.Inputs;

namespace _360o.Server.API.V1.Stores.Services
{
    public interface IStoresService
    {
        Task<Store> CreateStoreAsync(CreateStoreInput input);
        Task<Store?> GetStoreByIdByAsync(Guid storeId);
        Task<Store> UpdateStoreAsync(UpdateStoreInput input);
        Task<IList<Store>> ListStoresAsync(ListStoresInput input);
        Task DeleteStoreByIdAsync(Guid storeId);
        Task<Item> CreateItemAsync(CreateItemInput input);
        Task<Item?> GetItembyIdAsync(Guid itemId);
        Task<IList<Item>> ListItemsAsync(Guid storeId);
        Task DeleteItemByIdAsync(Guid itemId);
    }
}
