using _360o.Server.API.V1.Stores.Model;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace _360o.Server.API.V1.Stores.Services
{
    public class StoresService : IStoresService
    {
        private readonly ApiContext _apiContext;

        public StoresService(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<Store> CreateStoreAsync(CreateStoreInput input)
        {
            var store = new Store(
                input.OrganizationId,
                new Place(
                    input.Place.GooglePlaceId,
                    input.Place.FormattedAddress,
                    input.Place.Location
                    )
                );

            _apiContext.Stores.Add(store);

            await _apiContext.SaveChangesAsync();

            return store;
        }

        public async Task<Store?> GetStoreByIdByAsync(Guid id)
        {
            return await _apiContext.Stores.Include(s => s.Place).SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IList<Store>> ListStoresAsync(ListStoresInput input)
        {
            var stores = _apiContext.Stores.Include(m => m.Place).AsQueryable();

            if (input.Query != null)
            {
                stores = stores.Where(s => s.Organization.EnglishSearchVector.Matches(EF.Functions.WebSearchToTsQuery("english", input.Query)) || s.Organization.FrenchSearchVector.Matches(EF.Functions.WebSearchToTsQuery("french", input.Query)));
            }

            if (input.Latitude.HasValue && input.Longitude.HasValue && input.Radius.HasValue)
            {
                var locationPoint = new Point(x: input.Longitude.Value, y: input.Latitude.Value);
                stores = stores.Where(p => p.Place.Point.Distance(locationPoint) < input.Radius.Value);
            }

            return await stores.ToListAsync();

        }

        public async Task DeleteStoreByIdAsync(Guid id)
        {
            var store = await _apiContext.Stores.FindAsync(id);

            if (store == null)
            {
                throw new KeyNotFoundException("Store not found");
            }

            _apiContext.Stores.Remove(store);

            await _apiContext.SaveChangesAsync();
        }

        public async Task<Item> CreateItemAsync(CreateItemInput input)
        {
            var item = new Item(
                input.StoreId,
                input.Name,
                input.EnglishDescription,
                input.FrenchDescription,
                input.Price
                );

            _apiContext.Add(item);

            await _apiContext.SaveChangesAsync();

            return item;
        }

        public async Task<Item?> GetItembyIdAsync(Guid id)
        {
            return await _apiContext.Items.FindAsync(id);
        }
    }
}
