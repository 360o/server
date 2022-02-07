﻿using _360o.Server.Api.V1.Stores.Model;
using _360o.Server.Api.V1.Stores.Services.Inputs;

namespace _360o.Server.Api.V1.Stores.Services
{
    public interface IStoresService
    {
        Task<Store> CreateStoreAsync(CreateStoreInput input);

        Task<Store?> GetStoreByIdByAsync(Guid storeId);

        Task<Store> PatchStoreAsync(Guid storeId, PatchStoreInput input);

        Task<IList<Store>> ListStoresAsync(ListStoresInput input);

        Task DeleteStoreByIdAsync(Guid storeId);

        Task<Item> CreateItemAsync(CreateItemInput input);

        Task<Item?> GetItembyIdAsync(Guid itemId);

        Task<IList<Item>> ListItemsAsync(Guid storeId);

        Task<Item> PatchItemAsync(Guid itemId, PatchItemInput input);

        Task DeleteItemByIdAsync(Guid itemId);

        Task<Offer> CreateOfferAsync(Guid storeId, CreateOrUpdateOfferInput input);

        Task<Offer?> GetOfferByIdAsync(Guid offerId);

        Task<IList<Offer>> ListOffersAsync(Guid storeId);

        Task<Offer> UpdateOfferAsync(Guid offerId, CreateOrUpdateOfferInput input);
    }
}