using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public interface IStoresService
    {
        Task<Store> CreateStoreAsync(CreateStoreInput input);
        Task<Store?> GetStoreByIdByAsync(Guid id);
        Task<IList<Store>> ListStoresAsync(ListStoresInput input);
        Task DeleteStoreByIdAsync(Guid id);
        Task<Item> CreateItemAsync(CreateItemInput input);
        Task<Item?> GetItembyIdAsync(Guid id);
    }
}
