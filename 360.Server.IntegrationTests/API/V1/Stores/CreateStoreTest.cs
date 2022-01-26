using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class CreateStoreTest
    {
        [TestMethod]
        public async Task GivenValidRequestShouldReturnCreated()
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(Guid.TryParse(result.Id.ToString(), out var _));
            Assert.AreEqual($"/api/v1/Stores/{result.Id}", response.Headers.Location.AbsolutePath);

            Assert.AreEqual(request.DisplayName, result.DisplayName);

            Assert.AreEqual(request.EnglishShortDescription, result.EnglishShortDescription);
            Assert.AreEqual(request.EnglishLongDescription, result.EnglishLongDescription);
            Assert.IsTrue(result.EnglishCategories.SetEquals(request.EnglishCategories));

            Assert.AreEqual(request.FrenchLongDescription, result.FrenchLongDescription);
            Assert.AreEqual(request.FrenchLongDescription, result.FrenchLongDescription);
            Assert.IsTrue(result.FrenchCategories.SetEquals(request.FrenchCategories));

            Assert.IsTrue(Guid.TryParse(result.Place.Id.ToString(), out var _));
            Assert.AreEqual(request.Place.GooglePlaceId, result.Place.GooglePlaceId);
            Assert.AreEqual(request.Place.FormattedAddress, result.Place.FormattedAddress);
            Assert.AreEqual(request.Place.Location, result.Place.Location);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceDisplayNameShouldReturnBadRequest(string displayName)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.DisplayName = displayName;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("DisplayName"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceEnglishShortDescriptionShouldReturnCreated(string englishShortDescription)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.EnglishShortDescription = englishShortDescription;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent);

            Assert.IsNotNull(string.Empty, result.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceEnglishLongDescriptionShouldReturnCreated(string englishLongDescription)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.EnglishLongDescription = englishLongDescription;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent);

            Assert.IsNotNull(string.Empty, result.EnglishLongDescription);
        }

        [TestMethod]
        public async Task GivenNullEnglishCategoriesShouldReturnCreated()
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.EnglishCategories = null;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.AreEqual(0, result.EnglishCategories.Count);
        }

        [TestMethod]
        public async Task GivenEmptyEnglishCategoriesShouldReturnCreated()
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.EnglishCategories = new HashSet<string>();

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.AreEqual(0, result.EnglishCategories.Count);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFrenchShortDescriptionShouldReturnCreated(string frenchShortDescription)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.FrenchShortDescription = frenchShortDescription;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent);

            Assert.IsNotNull(string.Empty, result.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFrenchLongDescriptionShouldReturnCreated(string frenchLongDescription)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.FrenchLongDescription = frenchLongDescription;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent);

            Assert.IsNotNull(string.Empty, result.FrenchLongDescription);
        }

        [TestMethod]
        public async Task GivenNullFrenchCategoriesShouldReturnCreated()
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.FrenchCategories = null;

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.AreEqual(0, result.FrenchCategories.Count);
        }

        [TestMethod]
        public async Task GivenEmptyFrenchCategoriesShouldReturnCreated()
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.FrenchCategories = new HashSet<string>();

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.AreEqual(0, result.FrenchCategories.Count);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceGooglePlaceIdShouldReturnBadRequest(string googlePlaceId)
        {
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.Place = new CreateMerchantPlace
            {
                GooglePlaceId = googlePlaceId,
                FormattedAddress = request.Place.FormattedAddress,
                Location = request.Place.Location,
            };

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

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
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.Place = new CreateMerchantPlace
            {
                GooglePlaceId = request.Place.GooglePlaceId,
                FormattedAddress = formattedAddress,
                Location = request.Place.Location,
            };

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

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
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.Place = new CreateMerchantPlace
            {
                GooglePlaceId = request.Place.GooglePlaceId,
                FormattedAddress = request.Place.FormattedAddress,
                Location = new LocationDTO
                {
                    Latitude = latitude,
                    Longitude = request.Place.Location.Longitude
                }
            };

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

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
            var request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            request.Place = new CreateMerchantPlace
            {
                GooglePlaceId = request.Place.GooglePlaceId,
                FormattedAddress = request.Place.FormattedAddress,
                Location = new LocationDTO
                {
                    Latitude = request.Place.Location.Latitude,
                    Longitude = longitude
                }
            };

            var response = await ProgramTest.StoresHelper.CreateStoreAsync(request);

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
