using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class DeleteOfferByIdTest
    {
        [TestMethod]
        public async Task GivenOfferExistsShouldReturnNoContent()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var createOfferRequestItems = Generator.MakeRandomCreateOfferRequestItems(new List<Guid> { item.Id });

            var offer = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, createOfferRequestItems, null);

            var response = await ProgramTest.ApiClientUser1.Stores.DeleteOfferByIdAsync(store.Id, offer.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var getOfferResponse = await ProgramTest.ApiClientUser1.Stores.GetOfferByIdAsync(store.Id, offer.Id);

            Assert.AreEqual(HttpStatusCode.NotFound, getOfferResponse.StatusCode);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var response = await ProgramTest.NewClient().DeleteAsync(StoresHelper.OfferRoute(Guid.NewGuid(), Guid.NewGuid()));

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var createOfferRequestItems = Generator.MakeRandomCreateOfferRequestItems(new List<Guid> { item.Id });

            var offer = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, createOfferRequestItems, null);

            var response = await ProgramTest.ApiClientUser1.Stores.DeleteOfferByIdAsync(Guid.NewGuid(), offer.Id);

            await CustomAssertions.AssertNotFoundWithProblemDetailsAsync(response, "Store not found");
        }
    }
}