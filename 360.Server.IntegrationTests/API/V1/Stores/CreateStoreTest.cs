using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using Microsoft.AspNetCore.Mvc;
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
    public class CreateStoreTest
    {
        [TestMethod]
        public async Task GivenOrganizationExistsAndValidRequestShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var request = RequestsGenerator.MakeRandomCreateStoreRequest(organization.Id);

            var store = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(request);

            Assert.AreEqual(request.Place.GooglePlaceId, store.Place.GooglePlaceId);
            Assert.AreEqual(request.Place.FormattedAddress, store.Place.FormattedAddress);
            Assert.AreEqual(request.Place.Location, store.Place.Location);
            Assert.AreEqual(request.OrganizationId, store.Organization.Id);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(StoresHelper.StoresRoute, requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.NotFound.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.Status.Value);
            Assert.AreEqual("Organization not found", result.Detail);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var request = RequestsGenerator.MakeRandomCreateStoreRequest(organization.Id);

            var response = await ProgramTest.ApiClientUser2.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceGooglePlaceIdShouldReturnBadRequest(string googlePlaceId)
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO
                {
                    GooglePlaceId = googlePlaceId,
                    FormattedAddress = request.Place.FormattedAddress,
                    Location = request.Place.Location,
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("GooglePlaceId"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFormattedAddressShouldReturnBadRequest(string formattedAddress)
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO
                {
                    GooglePlaceId = request.Place.GooglePlaceId,
                    FormattedAddress = formattedAddress,
                    Location = request.Place.Location,
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("FormattedAddress"));
        }

        [DataTestMethod]
        [DataRow(-90 - double.MinValue)]
        [DataRow(90 + double.MinValue)]
        public async Task GivenLatitudeOutOfRangeShouldReturnBadRequest(double latitude)
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO
                {
                    GooglePlaceId = request.Place.GooglePlaceId,
                    FormattedAddress = request.Place.FormattedAddress,
                    Location = new Location(latitude, request.Place.Location.Longitude)
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Latitude"));
        }

        [DataTestMethod]
        [DataRow(-180 - double.MinValue)]
        [DataRow(180 + double.MinValue)]
        public async Task GivenLongitudeOutOfRangeShouldReturnBadRequest(double longitude)
        {
            var request = RequestsGenerator.MakeRandomCreateStoreRequest(Guid.NewGuid());

            request = request with
            {
                Place = new PlaceDTO
                {
                    GooglePlaceId = request.Place.GooglePlaceId,
                    FormattedAddress = request.Place.FormattedAddress,
                    Location = new Location(request.Place.Location.Latitude, longitude)
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Longitude"));
        }
    }
}
