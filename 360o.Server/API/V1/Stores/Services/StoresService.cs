using _360o.Server.Api.V1.Stores.Model;
using _360o.Server.Api.V1.Stores.Services.Inputs;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace _360o.Server.Api.V1.Stores.Services
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
            var store = new Store(input.OrganizationId);

            store.SetPlace(new Place(
                input.Place.GooglePlaceId,
                    input.Place.FormattedAddress,
                    input.Place.Location
                ));

            _apiContext.Stores.Add(store);

            await _apiContext.SaveChangesAsync();

            return store;
        }

        public async Task<Store?> GetStoreByIdByAsync(Guid storeId)
        {
            return await _apiContext.Stores
                .Where(s => !s.DeletedAt.HasValue)
                .Include(s => s.Place)
                .Include(s => s.Organization)
                .SingleOrDefaultAsync(s => s.Id == storeId);
        }

        public async Task<IList<Store>> ListStoresAsync(ListStoresInput input)
        {
            var stores = _apiContext.Stores
                .Where(s => !s.DeletedAt.HasValue)
                .Include(s => s.Place)
                .Include(s => s.Organization)
                .AsQueryable();

            if (input.Query != null)
            {
                stores = stores.Where(s =>
                s.Organization.EnglishSearchVector.Matches(EF.Functions.WebSearchToTsQuery("english", input.Query)) ||
                s.Organization.FrenchSearchVector.Matches(EF.Functions.WebSearchToTsQuery("french", input.Query)) ||
                s.Items.Any(
                    i => i.EnglishSearchVector.Matches(EF.Functions.WebSearchToTsQuery("english", input.Query)) ||
                    i.FrenchSearchVector.Matches(EF.Functions.WebSearchToTsQuery("french", input.Query)))
                );
            }

            if (input.Latitude.HasValue && input.Longitude.HasValue && input.Radius.HasValue)
            {
                var locationPoint = new Point(x: input.Longitude.Value, y: input.Latitude.Value);
                stores = stores.Where(p => p.Place.Point.Distance(locationPoint) < input.Radius.Value);
            }

            return await stores.ToListAsync();
        }

        public async Task<Store> UpdateStoreAsync(UpdateStoreInput input)
        {
            var store = await _apiContext.Stores.FindAsync(input.StoreId);

            if (store == null)
            {
                throw new KeyNotFoundException("Store not found");
            }

            if (input.Place != null)
            {
                store.SetPlace(input.Place);
            }

            await _apiContext.SaveChangesAsync();

            return store;
        }

        public async Task DeleteStoreByIdAsync(Guid storeId)
        {
            var store = await _apiContext.Stores.FindAsync(storeId);

            if (store == null)
            {
                throw new KeyNotFoundException("Store not found");
            }

            store.SetDelete();

            await _apiContext.SaveChangesAsync();
        }

        public async Task<Item> CreateItemAsync(CreateItemInput input)
        {
            var item = new Item(input.StoreId);

            if (input.EnglishName != null)
            {
                item.SetEnglishName(input.EnglishName);
            }

            if (input.EnglishDescription != null)
            {
                item.SetEnglishDescription(input.EnglishDescription);
            }

            if (input.FrenchName != null)
            {
                item.SetFrenchName(input.FrenchName);
            }

            if (input.FrenchDescription != null)
            {
                item.SetFrenchDescription(input.FrenchDescription);
            }

            if (input.Price.HasValue)
            {
                item.SetPrice(input.Price.Value);
            }

            _apiContext.Add(item);

            await _apiContext.SaveChangesAsync();

            return item;
        }

        public async Task<Item?> GetItembyIdAsync(Guid itemId)
        {
            return await _apiContext.Items
                .Include(i => i.Store)
                .Include(i => i.Store.Organization)
                .Where(i => !i.DeletedAt.HasValue)
                .SingleOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task<IList<Item>> ListItemsAsync(Guid storeId)
        {
            return await _apiContext.Items
                .Where(s => s.StoreId == storeId)
                .Where(s => !s.DeletedAt.HasValue)
                .ToListAsync();
        }

        public async Task<Item> UpdateItemAsync(UpdateItemInput input)
        {
            var item = await _apiContext.Items.FindAsync(input.ItemId);

            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }

            if (input.EnglishName != null)
            {
                item.SetEnglishName(input.EnglishName);
            }

            if (input.EnglishDescription != null)
            {
                item.SetEnglishDescription(input.EnglishDescription);
            }

            if (input.FrenchName != null)
            {
                item.SetFrenchName(input.FrenchName);
            }

            if (input.FrenchDescription != null)
            {
                item.SetFrenchDescription(input.FrenchDescription);
            }

            if (input.Price.HasValue)
            {
                item.SetPrice(input.Price.Value);
            }

            await _apiContext.SaveChangesAsync();

            return item;
        }

        public async Task DeleteItemByIdAsync(Guid storeId)
        {
            var item = await _apiContext.Items.FindAsync(storeId);

            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }

            item.SetDelete();

            await _apiContext.SaveChangesAsync();
        }

        public async Task<Offer> CreateOfferAsync(CreateOfferInput input)
        {
            var store = await GetStoreByIdByAsync(input.StoreId);

            if (store == null)
            {
                throw new KeyNotFoundException("Store not found");
            }

            var offer = new Offer(store.Id);

            if (input.EnglishName != null)
            {
                offer.SetEnglishName(input.EnglishName);
            }

            if (input.FrenchName != null)
            {
                offer.SetFrenchName(input.FrenchName);
            }

            var offerItems = new HashSet<OfferItem>();

            foreach (var inputItem in input.Items)
            {
                var item = await GetItembyIdAsync(inputItem.itemId);

                if (item == null)
                {
                    throw new KeyNotFoundException($"Item {inputItem.itemId} not found");
                }

                var offerItem = new OfferItem(item.Id);

                offerItem.SetQuantity(inputItem.Quantity);

                offerItems.Add(offerItem);
            }

            offer.SetOfferItems(offerItems);

            if (input.Discount.HasValue)
            {
                offer.SetDiscount(input.Discount.Value);
            }

            _apiContext.Offers.Add(offer);

            await _apiContext.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer?> GetOfferByIdAsync(Guid offerId)
        {
            return await _apiContext.Offers.FindAsync(offerId);
        }
    }
}