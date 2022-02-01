using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
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
    }
}
