using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class ListStoresTest
    {
        [TestMethod]
        public async Task GivenNoFilterShouldReturnAllStores()
        {
            var user1Store1 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest());
            var user1Store2 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest());
            var user2Store1 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest(), user: StoreUser.Two);

            var response = await ProgramTest.StoresHelper.ListStoresAsync(new ListStoresRequest());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<StoreDTO>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(result);
            var resultUser1Store1 = result.Find(s => s.Id == user1Store1.Id);
            var resultUser1Store2 = result.Find(s => s.Id == user1Store2.Id);
            var resultUser2Store1 = result.Find(s => s.Id == user2Store1.Id);
            ProgramTest.StoresHelper.AssertStoresEqual(user1Store1, resultUser1Store1);
            ProgramTest.StoresHelper.AssertStoresEqual(user1Store2, resultUser1Store2);
            ProgramTest.StoresHelper.AssertStoresEqual(user2Store1, resultUser2Store1);
        }
    }
}
