using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Stores.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient
{
    public class StoresHelper
    {
        private readonly IAuthHelper _authHelper;

        public StoresHelper(IAuthHelper authService)
        {
            _authHelper = authService;
        }

        public static string StoresRoute => "/api/v1/Stores";

        public static string StoreRoute(Guid storeId) => $"{StoresRoute}/{storeId}";

        public static string ItemsRoute(Guid storeId) => $"{StoresRoute}/{storeId}/items";

        public static string ItemRoute(Guid storeId, Guid itemId) => $"{ItemsRoute(storeId)}/{itemId}";

        public static string OffersRoute(Guid storeId) => $"{StoreRoute(storeId)}/offers";

        public static string OfferRoute(Guid storeId, Guid offerId) => $"{OffersRoute(storeId)}/{offerId}";

        public static void AssertOffersAreEqual(OfferDTO expected, OfferDTO actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            CollectionAssert.AreEquivalent(expected.OfferItems.ToList(), actual.OfferItems.ToList());
            Assert.AreEqual(expected.Discount, actual.Discount);
            Assert.AreEqual(expected.StoreId, actual.StoreId);
        }

        public async Task<HttpResponseMessage> CreateStoreAsync(CreateStoreRequest request)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(request);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(StoresRoute, requestContent);
        }

        public async Task<StoreDTO> CreateStoreAndDeserializeAsync(CreateStoreRequest request)
        {
            var response = await CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var store = await JsonUtils.DeserializeAsync<StoreDTO>(response);

            Assert.IsNotNull(store);
            Assert.IsTrue(Guid.TryParse(store.Id.ToString(), out var _));
            Assert.AreEqual(StoreRoute(store.Id), response.Headers.Location.AbsolutePath);

            return store;
        }

        public async Task<StoreDTO> CreateRandomStoreAndDeserializeAsync(Guid organizationId)
        {
            var request = Generator.MakeRandomCreateStoreRequest(organizationId);

            return await CreateStoreAndDeserializeAsync(request);
        }

        public async Task<HttpResponseMessage> GetStoreByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient().GetAsync(StoreRoute(id));
        }

        public async Task<StoreDTO> GetStoreByIdAndDeserializeAsync(Guid id)
        {
            var response = await GetStoreByIdAsync(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<StoreDTO>(response);
        }

        public async Task<HttpResponseMessage> ListStoresAsync(ListStoresRequest request)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (request.Query != null)
            {
                queryString.Add("query", request.Query);
            }

            if (request.Latitude != null)
            {
                queryString.Add("latitude", request.Latitude.ToString());
            }

            if (request.Longitude != null)
            {
                queryString.Add("longitude", request.Longitude.ToString());
            }

            if (request.Radius != null)
            {
                queryString.Add("radius", request.Radius.ToString());
            }

            var uri = StoresRoute;

            var qs = queryString.ToString();

            if (!string.IsNullOrEmpty(qs))
            {
                uri = $"{uri}?{qs}";
            }

            return await ProgramTest.NewClient().GetAsync(uri);
        }

        public async Task<IList<StoreDTO>> ListStoresAndDeserializeAsync(ListStoresRequest request)
        {
            var response = await ListStoresAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<List<StoreDTO>>(response);
        }

        public async Task<HttpResponseMessage> PatchStoreAsync(Guid storeId, object patchDoc)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PatchAsync(StoreRoute(storeId), requestContent);
        }

        public async Task<StoreDTO> PatchStoreAndDeserializeAsync(Guid storeId, object patchDoc)
        {
            var response = await PatchStoreAsync(storeId, patchDoc);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<StoreDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteStoreByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(StoreRoute(id));
        }

        public async Task<HttpResponseMessage> CreateItemAsync(Guid storeId, CreateItemRequest request)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(request);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(ItemsRoute(storeId), requestContent);
        }

        public async Task<ItemDTO> CreateItemAndDeserializeAsync(Guid storeId, CreateItemRequest request)
        {
            var response = await CreateItemAsync(storeId, request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var item = await JsonUtils.DeserializeAsync<ItemDTO>(response);

            Assert.IsNotNull(item);
            Assert.IsTrue(Guid.TryParse(item.Id.ToString(), out var _));
            Assert.AreEqual(ItemRoute(storeId, item.Id), response.Headers.Location.AbsolutePath);

            return item;
        }

        public async Task<ItemDTO> CreateRandomItemAndDeserializeAsync(Guid storeId)
        {
            var request = Generator.MakeRandomCreateItemRequest();

            return await CreateItemAndDeserializeAsync(storeId, request);
        }

        public async Task<HttpResponseMessage> GetItemByIdAsync(Guid storeId, Guid itemId)
        {
            return await ProgramTest.NewClient().GetAsync(ItemRoute(storeId, itemId));
        }

        public async Task<ItemDTO> GetItemByIdAndDeserializeAsync(Guid storeId, Guid itemId)
        {
            var response = await GetItemByIdAsync(storeId, itemId);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<ItemDTO>(response);
        }

        public async Task<HttpResponseMessage> ListItemsAsync(Guid storeId)
        {
            return await ProgramTest.NewClient().GetAsync(ItemsRoute(storeId));
        }

        public async Task<IList<ItemDTO>> ListItemsAndDeserializeAsync(Guid storeId)
        {
            var response = await ListItemsAsync(storeId);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<IList<ItemDTO>>(response);
        }

        public async Task<HttpResponseMessage> PatchItemAsync(Guid storeId, Guid itemId, object patchDoc)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PatchAsync(ItemRoute(storeId, itemId), requestContent);
        }

        public async Task<ItemDTO> PatchItemAndDeserializeAsync(Guid storeId, Guid itemId, object patchDoc)
        {
            var response = await PatchItemAsync(storeId, itemId, patchDoc);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<ItemDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteItemByIdAsync(Guid storeId, Guid itemId)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(ItemRoute(storeId, itemId));
        }

        public async Task<HttpResponseMessage> CreateOfferAsync(Guid storeId, CreateOfferRequest request)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(request);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(OffersRoute(storeId), requestContent);
        }

        public async Task<OfferDTO> CreateRandomOfferAndDeserializeAsync(Guid storeId, IEnumerable<CreateOfferRequestItem> offerItems, MoneyValueDTO? discount)
        {
            var request = Generator.MakeRandomCreateOfferRequest(offerItems, discount);

            var response = await CreateOfferAsync(storeId, request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var offer = await JsonUtils.DeserializeAsync<OfferDTO>(response);

            Assert.IsNotNull(offer);
            Assert.IsTrue(Guid.TryParse(offer.Id.ToString(), out var _));
            Assert.AreEqual(OfferRoute(storeId, offer.Id), response.Headers.Location.AbsolutePath);

            return offer;
        }

        public async Task<HttpResponseMessage> GetOfferByIdAsync(Guid storeId, Guid offerId)
        {
            return await ProgramTest.NewClient().GetAsync(OfferRoute(storeId, offerId));
        }

        public async Task<OfferDTO> GetOfferByIdAndDeserializeAsync(Guid storeId, Guid offerId)
        {
            var response = await GetOfferByIdAsync(storeId, offerId);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<OfferDTO>(response);
        }

        public async Task<HttpResponseMessage> ListOffersAsync(Guid storeId)
        {
            return await ProgramTest.NewClient().GetAsync(OffersRoute(storeId));
        }

        public async Task<IList<OfferDTO>> ListOffersAndDeserializeAsync(Guid storeId)
        {
            var response = await ListOffersAsync(storeId);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<IList<OfferDTO>>(response);
        }

        public async Task<HttpResponseMessage> DeleteOfferByIdAsync(Guid storeId, Guid offerId)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(OfferRoute(storeId, offerId));
        }
    }
}