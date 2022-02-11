using _360.Server.IntegrationTests.Api.V1.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class GetStoreByIdTest
    {
        [TestMethod]
        public async Task GivenStoreExistsShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var createdStore = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var store = await ProgramTest.ApiClientUser1.Stores.GetStoreByIdAndDeserializeAsync(createdStore.Id);

            CustomAssertions.AssertDTOsAreEqual(createdStore, store);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.GetStoreByIdAsync(Guid.NewGuid());

            await CustomAssertions.AssertNotFoundAsync(response, "Store not found");
        }
    }
}