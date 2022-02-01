using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360o.Server.API.V1.Errors.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Organizations
{
    [TestClass]
    public class DeleteOrganizationByIdTest
    {
        [TestMethod]
        public async Task GivenOrganizationExistsShouldReturnNoContent()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var response = await ProgramTest.ApiClientUser1.Organizations.DeleteOrganizationByIdAsync(organization.Id);

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var getOrganizationResponse = await ProgramTest.ApiClientUser1.Organizations.GetOrganizationByIdAsync(organization.Id);

            Assert.AreEqual(HttpStatusCode.NotFound, getOrganizationResponse.StatusCode);
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var response = await ProgramTest.NewClient().DeleteAsync($"{OrganizationsHelper.OrganizationsRoute}/{Guid.NewGuid()}");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var response = await ProgramTest.ApiClientUser2.Organizations.DeleteOrganizationByIdAsync(organization.Id);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Organizations.DeleteOrganizationByIdAsync(Guid.NewGuid());

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
