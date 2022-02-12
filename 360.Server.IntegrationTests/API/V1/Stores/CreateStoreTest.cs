using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Stores.DTOs;
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
    public class CreateStoreTest
    {
        [TestMethod]
        public async Task GivenOrganizationExistsAndValidRequestShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var request = Generator.MakeRandomCreateStoreRequest(organization.Id);

            var store = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(request);

            Assert.AreEqual(request.Place.GooglePlaceId, store.Place.GooglePlaceId);
            Assert.AreEqual(request.Place.FormattedAddress, store.Place.FormattedAddress);
            Assert.AreEqual(request.Place.Location, store.Place.Location);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(StoresHelper.StoresRoute, requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            await CustomAssertions.AssertNotFoundWithProblemDetailsAsync(response, "Organization not found");
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var request = Generator.MakeRandomCreateStoreRequest(organization.Id);

            var response = await ProgramTest.ApiClientUser2.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceGooglePlaceIdShouldReturnBadRequest(string googlePlaceId)
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO(googlePlaceId, request.Place.FormattedAddress, request.Place.Location)
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "GooglePlaceId");
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFormattedAddressShouldReturnBadRequest(string formattedAddress)
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO(request.Place.GooglePlaceId, formattedAddress, request.Place.Location)
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "FormattedAddress");
        }

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        public async Task GivenInvalidLatitudeShouldReturnBadRequest(double latitude)
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO(request.Place.GooglePlaceId, request.Place.FormattedAddress, new LocationDTO(latitude, request.Place.Location.Longitude))
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Latitude");
        }

        [DataTestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        public async Task GivenInvalidLongitudeShouldReturnBadRequest(double longitude)
        {
            var request = Generator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO(request.Place.GooglePlaceId, request.Place.FormattedAddress, new LocationDTO(request.Place.Location.Latitude, longitude))
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Longitude");
        }
    }
}