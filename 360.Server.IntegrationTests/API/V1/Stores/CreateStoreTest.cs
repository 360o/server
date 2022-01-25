using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
    }
}
