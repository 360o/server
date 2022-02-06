using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class UpdateStoreTest
    {
        [TestMethod]
        public async Task GivenPlaceIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var faker = new Faker();

            var request = new UpdateStoreRequest
            {
                Place = new PlaceDTO
                {
                    GooglePlaceId = faker.Random.Uuid().ToString(),
                    FormattedAddress = faker.Address.FullAddress(),
                    Location = new Location
                    {
                        Latitude = faker.Address.Latitude(),
                        Longitude = faker.Address.Longitude(),
                    }
                }
            };

            var updatedStore = await ProgramTest.ApiClientUser1.Stores.UpdateStoreAndDeserializeAsync(store.Id, request);

            Assert.AreEqual(request.Place, updatedStore.Place);
            StoresHelper.AssertStoresAreEqual(store, updatedStore with { Place = store.Place });
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = new UpdateStoreRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PatchAsync(StoresHelper.StoreRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = new UpdateStoreRequest();

            var response = await ProgramTest.ApiClientUser2.Stores.UpdateStoreAsync(store.Id, request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var request = new UpdateStoreRequest();

            var response = await ProgramTest.ApiClientUser1.Stores.UpdateStoreAsync(Guid.NewGuid(), request);

            await ProblemDetailAssertions.AssertNotFoundAsync(response, "Store not found");
        }
    }
}