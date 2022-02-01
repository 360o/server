using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class CreateItemTest
    {
        [TestMethod]
        public async Task GivenStoreExistsAndValidRequestShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = RequestsGenerator.MakeRandomCreateItemRequest();

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.AreEqual(request.Name, item.Name);
            Assert.AreEqual(request.EnglishDescription, item.EnglishDescription);
            Assert.AreEqual(request.FrenchDescription, item.FrenchDescription);
            Assert.AreEqual(request.Price, item.Price);
            Assert.AreEqual(store.Id, item.StoreId);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = RequestsGenerator.MakeRandomCreateItemRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(StoresHelper.ItemsRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
