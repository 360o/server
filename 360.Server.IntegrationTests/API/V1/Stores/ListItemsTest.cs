using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class ListItemsTest
    {
        [TestMethod]
        public async Task ShouldReturnAllItemsFromStore()
        {
            var user1Organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();
            var user2Organization = await ProgramTest.ApiClientUser2.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2Organization.Id);

            var user1Store1Item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(user1Store1.Id);
            var user1Store1Item2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(user1Store1.Id);
            var user1Store2Item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(user1Store2.Id);
            var user1Store2Item2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(user1Store2.Id);
            var user2Store1Item1 = await ProgramTest.ApiClientUser2.Stores.CreateRandomItemAndDeserializeAsync(user2Store1.Id);
            var user2Store1Item2 = await ProgramTest.ApiClientUser2.Stores.CreateRandomItemAndDeserializeAsync(user2Store1.Id);

            var expectedToContainItems = new Dictionary<Guid, ItemDTO>
            {
                { user1Store1Item1.Id, user1Store1Item1 },
                { user1Store1Item2.Id, user1Store1Item2 }
            };

            var expectToNotContainItems = new List<ItemDTO>
            {
                user1Store2Item1,
                user1Store2Item2,
                user1Store2Item1,
                user1Store2Item2
            };

            var items = await ProgramTest.ApiClientUser1.Stores.ListItemsAndDeserializeAsync(user1Store1.Id);

            var itemsDict = items.ToDictionary(i => i.Id, i => i);

            Assert.AreEqual(expectedToContainItems[user1Store1Item1.Id], itemsDict[user1Store1Item1.Id]);
            Assert.AreEqual(expectedToContainItems[user1Store1Item2.Id], itemsDict[user1Store1Item2.Id]);
            foreach (var item in expectToNotContainItems)
            {
                Assert.IsFalse(itemsDict.ContainsKey(item.Id));
            }
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotfound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListItemsAsync(Guid.NewGuid());

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.ItemNotFound.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.Status.Value);
            Assert.AreEqual("Store not found", result.Detail);
        }
    }
}
