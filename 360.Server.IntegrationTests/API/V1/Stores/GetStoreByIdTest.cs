using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var createdMerchant = await _storesHelper.CreateMerchantAsync(createMerchantRequest);

            var result = await _storesHelper.GetMerchantByIdAsync(createdMerchant.Id);

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
