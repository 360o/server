using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using _360o.Server.API.V1.Stores.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    public class StoresHelper
    {
        public static string StoresRoute => "/api/v1/Stores";
        public static string StoreRoute(Guid storeId) => $"{StoresRoute}/{storeId}";
        public static string ItemsRoute(Guid storeId) => $"{StoresRoute}/{storeId}/items";
        public static string ItemRoute(Guid storeId, Guid itemId) => $"{ItemsRoute(storeId)}/{itemId}";
        public static void AssertStoresAreEqual(StoreDTO expected, StoreDTO actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Place, actual.Place);
            OrganizationsHelper.AssertOrganizationsAreEqual(expected.Organization, actual.Organization);
        }

        private readonly IAuthHelper _authHelper;

        public StoresHelper(IAuthHelper authService)
        {
            _authHelper = authService;
        }

        public async Task<HttpResponseMessage> CreateStoreAsync(CreateStoreRequest request)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(StoresRoute, requestContent);
        }

        public async Task<StoreDTO> CreateStoreAndDeserializeAsync(CreateStoreRequest request)
        {
            var response = await CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var store = await Utils.DeserializeAsync<StoreDTO>(response);

            Assert.IsTrue(Guid.TryParse(store.Id.ToString(), out var _));
            Assert.AreEqual(StoreRoute(store.Id), response.Headers.Location.AbsolutePath);

            return store;
        }

        public async Task<StoreDTO> CreateRandomStoreAndDeserializeAsync(Guid organizationId)
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(organizationId);

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

            return await Utils.DeserializeAsync<StoreDTO>(response);
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

            return await Utils.DeserializeAsync<List<StoreDTO>>(response);
        }

        public async Task<HttpResponseMessage> UpdateStoreAsync(Guid storeId, UpdateStoreRequest request)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PatchAsync(StoreRoute(storeId), requestContent);
        }

        public async Task<StoreDTO> UpdateStoreAndDeserializeAsync(Guid storeId, UpdateStoreRequest request)
        {
            var response = await UpdateStoreAsync(storeId, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await Utils.DeserializeAsync<StoreDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteStoreByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(StoreRoute(id));
        }

        public async Task<HttpResponseMessage> CreateItemAsync(Guid storeId, CreateItemRequest request)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(ItemsRoute(storeId), requestContent);
        }

        public async Task<ItemDTO> CreateItemAndDeserializeAsync(Guid storeId, CreateItemRequest request)
        {
            var response = await CreateItemAsync(storeId, request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var item = await Utils.DeserializeAsync<ItemDTO>(response);

            Assert.IsNotNull(item);
            Assert.IsTrue(Guid.TryParse(item.Id.ToString(), out var _));
            Assert.AreEqual(ItemRoute(storeId, item.Id), response.Headers.Location.AbsolutePath);

            return item;
        }

        public async Task<ItemDTO> CreateRandomItemAndDeserializeAsync(Guid storeId)
        {
            var request = RequestsGenerator.MakeRandomCreateItemRequest();

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

            return await Utils.DeserializeAsync<ItemDTO>(response);
        }

        public async Task<HttpResponseMessage> ListItemsAsync(Guid storeId)
        {
            return await ProgramTest.NewClient().GetAsync(ItemsRoute(storeId));
        }

        public async Task<IList<ItemDTO>> ListItemsAndDeserializeAsync(Guid storeId)
        {
            var response = await ListItemsAsync(storeId);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await Utils.DeserializeAsync<IList<ItemDTO>>(response);
        }

        public async Task<HttpResponseMessage> DeleteItemByIdAsync(Guid storeId, Guid itemId)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(ItemRoute(storeId, itemId));
        }
    }
}
