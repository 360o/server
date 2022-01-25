using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class GetStoreByIdTest
    {
        private readonly StoresHelper _storesHelper;

        public GetStoreByIdTest()
        {
            _storesHelper = new StoresHelper(new AccessTokenHelper());
        }

        [TestMethod]
        public async Task GivenStoreExistsShouldReturnOK()
        {
            var createMerchantRequest = _storesHelper.MakeRandomCreateMerchantRequest();

            var createdMerchantResponse = await _storesHelper.CreateStoreAsync(createMerchantRequest);

            Assert.AreEqual(HttpStatusCode.Created, createdMerchantResponse.StatusCode);

            var createdMerchantResponseContent = await createdMerchantResponse.Content.ReadAsStringAsync();

            var createdMerchant = JsonSerializer.Deserialize<StoreDTO>(createdMerchantResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var response = await _storesHelper.GetStoreByIdAsync(createdMerchant.Id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.AreEqual(createdMerchant.Id, result.Id);
            Assert.AreEqual(createdMerchant.DisplayName, result.DisplayName);
            Assert.AreEqual(createdMerchant.EnglishShortDescription, result.EnglishShortDescription);
            Assert.AreEqual(createdMerchant.EnglishLongDescription, result.EnglishLongDescription);
            Assert.IsTrue(result.EnglishCategories.SetEquals(createdMerchant.EnglishCategories));
            Assert.AreEqual(createdMerchant.FrenchShortDescription, result.FrenchShortDescription);
            Assert.AreEqual(createdMerchant.FrenchLongDescription, result.FrenchLongDescription);
            Assert.IsTrue(result.FrenchCategories.SetEquals(createdMerchant.FrenchCategories));
            Assert.AreEqual(createdMerchant.Place, result.Place);
        }
    }
}
