using _360.Server.IntegrationTests.Api.V1.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Organizations
{
    [TestClass]
    public class GetOrganizationByIdTest
    {
        [TestMethod]
        public async Task GivenOrganizationExistsShouldReturnOK()
        {
            var createdOrganization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var organization = await ProgramTest.ApiClientUser1.Organizations.GetOrganizationByIdAndDeserializeAsync(createdOrganization.Id);

            CustomAssertions.AssertSerializeToSameJson(createdOrganization, organization);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Organizations.GetOrganizationByIdAsync(Guid.NewGuid());

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}