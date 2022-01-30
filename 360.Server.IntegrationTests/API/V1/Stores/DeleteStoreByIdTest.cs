//using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
//using _360o.Server.API.V1.Errors.Enums;
//using _360o.Server.API.V1.Stores.Controllers.DTOs;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Net;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace _360.Server.IntegrationTests.API.V1.Stores
//{
//    [TestClass]
//    public class DeleteStoreByIdTest
//    {
//        [TestMethod]
//        public async Task GivenStoreExistsShouldReturnNoContent()
//        {
//            var createMerchantRequest = RequestsGenerator.RandomCreateMerchantRequest();

//            var createMerchantResponse = await ProgramTest.StoresHelper.CreateStoreAsync(createMerchantRequest);

//            Assert.AreEqual(HttpStatusCode.Created, createMerchantResponse.StatusCode);

//            var createMerchantResponseContent = await createMerchantResponse.Content.ReadAsStringAsync();

//            var createdMerchant = JsonSerializer.Deserialize<StoreDTO>(createMerchantResponseContent, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true,
//            });

//            var response = await ProgramTest.StoresHelper.DeleteStoreByIdAsync(createdMerchant.Id);

//            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

//            var getMerchantResponse = await ProgramTest.StoresHelper.GetStoreByIdAsync(createdMerchant.Id);

//            Assert.AreEqual(HttpStatusCode.NotFound, getMerchantResponse.StatusCode);
//        }

//        [TestMethod]
//        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
//        {
//            var response = await ProgramTest.StoresHelper.DeleteStoreByIdAsync(Guid.NewGuid());

//            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

//            var responseContent = await response.Content.ReadAsStringAsync();

//            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

//            Assert.IsNotNull(result);
//            Assert.IsNotNull(result.Status);
//            Assert.AreEqual(ErrorCode.ItemNotFound.ToString(), result.Title);
//            Assert.AreEqual((int)HttpStatusCode.NotFound, result.Status.Value);
//        }
//    }
//}
