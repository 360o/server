using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class CreateStoreTest
    {
        private readonly StoresHelper _storesHelper;

        public CreateStoreTest()
        {
            _storesHelper = new StoresHelper(new AccessTokenHelper());
        }

        [TestMethod]
        public async Task GivenValidRequestShouldReturnCreated()
        {
            var request = _storesHelper.MakeRandomCreateMerchantRequest();

            var response = await _storesHelper.CreateStoreAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(Guid.TryParse(result.Id.ToString(), out var _));

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
        public async Task GivenNullOrEmptyDisplayNameShouldReturnBadRequest(string displayName)
        {
            var request = _storesHelper.MakeRandomCreateMerchantRequest();

            request.DisplayName = displayName;

            var response = await _storesHelper.CreateStoreAsync(request);

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
    }
}
