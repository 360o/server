using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

        [TestMethod]
        public async Task GivenQueryParamShouldReturnStoresWithDisplayNameOrDescriptionsOrCategoriesMatchingTheQuery()
        {
            var myQuery = "amigurumi";

            var user1Store1Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user1Store2Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user1Store3Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user1Store4Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user2Store1Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user2Store2Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();
            var user2Store3Request = ProgramTest.StoresHelper.MakeRandomCreateMerchantRequest();

            user1Store1Request.DisplayName = $"{user1Store1Request.DisplayName} {myQuery}";
            user1Store2Request.EnglishShortDescription = $"{user1Store2Request.EnglishShortDescription} {myQuery}";
            user1Store3Request.EnglishLongDescription = $"{user1Store3Request.EnglishLongDescription} {myQuery}";
            user1Store4Request.EnglishCategories.Add(myQuery);
            user2Store1Request.FrenchShortDescription = $"{user2Store1Request.FrenchShortDescription} {myQuery}";
            user2Store2Request.FrenchLongDescription = $"{user2Store2Request.FrenchLongDescription} {myQuery}";
            user2Store3Request.FrenchCategories.Add(myQuery);


            var user1Store1 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user1Store1Request);
            var user1Store2 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user1Store2Request);
            var user1Store3 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user1Store3Request);
            var user1Store4 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user1Store4Request);
            var user2Store1 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user2Store1Request);
            var user2Store2 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user2Store2Request);
            var user2Store3 = await ProgramTest.StoresHelper.CreateStoreAndDeserializeAsync(user2Store3Request);

            var expectedStores = new Dictionary<Guid, StoreDTO>()
            {
                { user1Store1.Id, user1Store1 },
                { user1Store2.Id, user1Store2 },
                { user1Store3.Id, user1Store3 },
                { user1Store4.Id, user1Store4 },
                { user2Store1.Id, user2Store1 },
                { user2Store2.Id, user2Store2 },
                { user2Store3.Id, user2Store3 }
            };

            var response = await ProgramTest.StoresHelper.ListStoresAsync(new ListStoresRequest
            {
                Query = myQuery
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<StoreDTO>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedStores.Count, result.Count);
            foreach (var store in result)
            {
                ProgramTest.StoresHelper.AssertStoresEqual(expectedStores[store.Id], store);
            }
        }
    }
}
