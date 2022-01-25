using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class DeleteStoreByIdTest
    {
        private readonly StoresHelper _storesHelper;

        public DeleteStoreByIdTest()
        {
            _storesHelper = new StoresHelper(new AccessTokenHelper());
        }

        [TestMethod]
        public async Task GivenStoreExistsShouldReturnNoContent()
        {
            var createMerchantRequest = _storesHelper.MakeRandomCreateMerchantRequest();

            var createMerchantResponse = await _storesHelper.CreateStoreAsync(createMerchantRequest);

            Assert.AreEqual(HttpStatusCode.Created, createMerchantResponse.StatusCode);

            var createMerchantResponseContent = await createMerchantResponse.Content.ReadAsStringAsync();

            var createdMerchant = JsonSerializer.Deserialize<StoreDTO>(createMerchantResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            var response = await _storesHelper.DeleteStoreByIdAsync(createdMerchant.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var getMerchantResponse = await _storesHelper.GetStoreByIdAsync(createdMerchant.Id);

            Assert.AreEqual(HttpStatusCode.NotFound, getMerchantResponse.StatusCode);
        }
    }
}
