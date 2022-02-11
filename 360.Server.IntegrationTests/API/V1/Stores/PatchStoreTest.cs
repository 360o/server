using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Stores.DTOs;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class PatchStoreTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public async Task GivenPlaceShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = new PlaceDTO
            {
                GooglePlaceId = _faker.Random.Uuid().ToString(),
                FormattedAddress = _faker.Address.FullAddress(),
                Location = new LocationDTO(_faker.Address.Latitude(), _faker.Address.Longitude())
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = place
                }
            };

            var updatedStore = await ProgramTest.ApiClientUser1.Stores.PatchStoreAndDeserializeAsync(store.Id, patchDoc);

            Assert.AreEqual(place, updatedStore.Place);
            CustomAssertions.AssertDTOsAreEqual(store, updatedStore with { Place = store.Place });
        }

        [TestMethod]
        public async Task GivenNullGooglePlaceIdShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = new PlaceDTO
            {
                GooglePlaceId = null,
                FormattedAddress = _faker.Address.FullAddress(),
                Location = new LocationDTO(_faker.Address.Latitude(), _faker.Address.Longitude())
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = place
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceGooglePlaceIdShouldReturnBadRequest(string googlePlaceId)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = new PlaceDTO
            {
                GooglePlaceId = googlePlaceId,
                FormattedAddress = _faker.Address.FullAddress(),
                Location = new LocationDTO(_faker.Address.Latitude(), _faker.Address.Longitude())
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = place
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenNullFormattedAddressShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = new PlaceDTO
            {
                GooglePlaceId = _faker.Random.Uuid().ToString(),
                FormattedAddress = null,
                Location = new LocationDTO(_faker.Address.Latitude(), _faker.Address.Longitude())
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = place
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceFormattedAddressShouldReturnBadRequest(string formattedAddress)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = new PlaceDTO
            {
                GooglePlaceId = _faker.Random.Uuid().ToString(),
                FormattedAddress = formattedAddress,
                Location = new LocationDTO(_faker.Address.Latitude(), _faker.Address.Longitude())
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = place
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var patchDoc = new[]
            {
                new
                {
                }
            };

            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            var response = await ProgramTest.NewClient().PatchAsync(StoresHelper.StoreRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser2.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(Guid.NewGuid(), patchDoc);

            await CustomAssertions.AssertNotFoundAsync(response, "Store not found");
        }
    }
}