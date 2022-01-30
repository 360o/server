using _360o.Server.API.V1.Errors.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class DeleteStoreByIdTest
    {
        [TestMethod]
        public async Task GivenStoreExistsShouldReturnNoContent()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.DeleteStoreByIdAsync(store.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.DeleteStoreByIdAsync(Guid.NewGuid());

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.ItemNotFound.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.Status.Value);
        }
    }
}
