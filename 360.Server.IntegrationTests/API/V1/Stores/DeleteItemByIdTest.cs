using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class DeleteItemByIdTest
    {
        [TestMethod]
        public async Task GivenItemExistsShouldReturnNoContent()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.DeleteItemByIdAsync(store.Id, item.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var getItemResponse = await ProgramTest.ApiClientUser1.Stores.GetItemByIdAsync(store.Id, item.Id);

            Assert.AreEqual(HttpStatusCode.NotFound, getItemResponse.StatusCode);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var response = await ProgramTest.NewClient().DeleteAsync(StoresHelper.ItemRoute(Guid.NewGuid(), Guid.NewGuid()));

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
