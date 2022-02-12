using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class DeleteStoreByIdTest
    {
        [TestMethod]
        public async Task GivenStoreExistsShouldReturnNoContent()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.DeleteStoreByIdAsync(store.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var getStoreResponse = await ProgramTest.ApiClientUser1.Stores.GetStoreByIdAsync(store.Id);

            Assert.AreEqual(HttpStatusCode.NotFound, getStoreResponse.StatusCode);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var response = await ProgramTest.NewClient().DeleteAsync(StoresHelper.StoreRoute(Guid.NewGuid()));

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var response = await ProgramTest.ApiClientUser2.Stores.DeleteStoreByIdAsync(store.Id);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.DeleteStoreByIdAsync(Guid.NewGuid());

            await CustomAssertions.AssertNotFoundWithProblemDetailsAsync(response, "Store not found");
        }
    }
}