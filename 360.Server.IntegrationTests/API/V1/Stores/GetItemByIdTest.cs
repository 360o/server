using _360.Server.IntegrationTests.Api.V1.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class GetItemByIdTest
    {
        [TestMethod]
        public async Task GivenItemExistsShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var createdItem = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.GetItemByIdAndDeserializeAsync(store.Id, createdItem.Id);

            Assert.AreEqual(createdItem, item);
        }

        [TestMethod]
        public async Task GivenItemDoesNotExistShouldReturnNotFound()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.GetItemByIdAsync(store.Id, Guid.NewGuid());

            await CustomAssertions.AssertNotFoundAsync(response, "Item not found");
        }
    }
}