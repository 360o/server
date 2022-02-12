using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class PatchStoreTest
    {
        [TestMethod]
        public async Task GivenPlaceShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = Generator.MakeRandomPlace();

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
            CustomAssertions.AssertSerializeToSameJson(store, updatedStore with { Place = store.Place });
        }

        [TestMethod]
        public async Task GivenNullGooglePlaceIdShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = Generator.MakeRandomPlace() with
            {
                GooglePlaceId = null
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

            var place = Generator.MakeRandomPlace() with
            {
                GooglePlaceId = googlePlaceId,
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

            var place = Generator.MakeRandomPlace() with
            {
                FormattedAddress = null
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

            var place = Generator.MakeRandomPlace() with
            {
                FormattedAddress = formattedAddress,
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
        public async Task GivenNullLocationShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = Generator.MakeRandomPlace();

            var patchDocPlace = new
            {
                GooglePlaceId = place.GooglePlaceId,
                FormattedAddress = place.FormattedAddress,
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = patchDocPlace
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchStoreAsync(store.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenNullLatitudeShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var patchDocPlace = new
            {
                GooglePlaceId = store.Place.GooglePlaceId,
                FormattedAddress = store.Place.FormattedAddress,
                Location = new
                {
                    Longitude = store.Place.Location.Longitude,
                }
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = patchDocPlace
                }
            };

            var updatedStore = await ProgramTest.ApiClientUser1.Stores.PatchStoreAndDeserializeAsync(store.Id, patchDoc);

            Assert.AreEqual(0, updatedStore.Place.Location.Latitude);
            CustomAssertions.AssertSerializeToSameJson(store, updatedStore with
            {
                Place = updatedStore.Place with
                {
                    Location = updatedStore.Place.Location with
                    {
                        Latitude = store.Place.Location.Latitude
                    }
                }
            });
        }

        [DataTestMethod]
        [DataRow(90 - double.MinValue)]
        [DataRow(90 + double.MinValue)]
        public async Task GivenInvalidLatitudeShouldReturnBadRequest(double latitude)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = Generator.MakeRandomPlace() with
            {
                Location = Generator.MakeRandomLocation() with
                {
                    Latitude = latitude,
                }
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
        public async Task GivenNullLongitudeShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var patchDocPlace = new
            {
                GooglePlaceId = store.Place.GooglePlaceId,
                FormattedAddress = store.Place.FormattedAddress,
                Location = new
                {
                    Latitude = store.Place.Location.Latitude,
                }
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Place",
                    value = patchDocPlace
                }
            };

            var updatedStore = await ProgramTest.ApiClientUser1.Stores.PatchStoreAndDeserializeAsync(store.Id, patchDoc);

            Assert.AreEqual(0, updatedStore.Place.Location.Longitude);
            CustomAssertions.AssertSerializeToSameJson(store, updatedStore with
            {
                Place = updatedStore.Place with
                {
                    Location = updatedStore.Place.Location with
                    {
                        Longitude = store.Place.Location.Longitude
                    }
                }
            });
        }

        [DataTestMethod]
        [DataRow(180 - double.MinValue)]
        [DataRow(180 + double.MinValue)]
        public async Task GivenInvalidLongitudeShouldReturnBadRequest(double longitude)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var place = Generator.MakeRandomPlace() with
            {
                Location = Generator.MakeRandomLocation() with
                {
                    Longitude = longitude,
                }
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

            await CustomAssertions.AssertNotFoundWithProblemDetailsAsync(response, "Store not found");
        }
    }
}