using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class PatchItemTest
    {
        private readonly Faker _englishFaker = new Faker();
        private readonly Faker _frenchFaker = new Faker("fr");

        [TestMethod]
        public async Task GivenEnglishNameShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var englishName = _englishFaker.Commerce.ProductName();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishName",
                    value = englishName
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.PatchItemAndDeserializeAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(englishName, updatedItem.EnglishName);
            Assert.AreEqual(item, updatedItem with { EnglishName = item.EnglishName });
        }

        [TestMethod]
        public async Task GivenEnglishDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var englishDescription = _englishFaker.Commerce.ProductDescription();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishDescription",
                    value = englishDescription
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.PatchItemAndDeserializeAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(englishDescription, updatedItem.EnglishDescription);
            Assert.AreEqual(item, updatedItem with { EnglishDescription = item.EnglishDescription });
        }

        [TestMethod]
        public async Task GivenFrenchNameShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var frenchName = _frenchFaker.Commerce.ProductName();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchName",
                    value = frenchName
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.PatchItemAndDeserializeAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(frenchName, updatedItem.FrenchName);
            Assert.AreEqual(item, updatedItem with { FrenchName = item.FrenchName });
        }

        [TestMethod]
        public async Task GivenFrenchDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var frenchDescription = _frenchFaker.Commerce.ProductDescription();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchDescription",
                    value = frenchDescription
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.PatchItemAndDeserializeAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(frenchDescription, updatedItem.FrenchDescription);
            Assert.AreEqual(item, updatedItem with { FrenchDescription = item.FrenchDescription });
        }

        [TestMethod]
        public async Task GivenPriceShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var price = new MoneyValue
            {
                Amount = _englishFaker.Random.Decimal(0, 100),
                CurrencyCode = _englishFaker.PickRandom<Iso4217CurrencyCode>()
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Price",
                    value = price
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.PatchItemAndDeserializeAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(price, updatedItem.Price);
            Assert.AreEqual(item, updatedItem with { Price = item.Price });
        }

        [TestMethod]
        public async Task GivenNegativePriceShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var price = new MoneyValue
            {
                Amount = _englishFaker.Random.Decimal(decimal.MinValue, -1),
                CurrencyCode = _englishFaker.PickRandom<Iso4217CurrencyCode>()
            };

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Price",
                    value = price
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchItemAsync(store.Id, item.Id, patchDoc);

            await CustomAssertions.AssertBadRequestAsync(response, "Amount");
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var patchDoc = new[]
            {
                new
                {
                }
            };

            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            var response = await ProgramTest.NewClient().PatchAsync(StoresHelper.ItemRoute(Guid.NewGuid(), Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenItemDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser2.Stores.PatchItemAsync(store.Id, item.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenItemDoesNotExistShouldReturnNotFound()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.PatchItemAsync(store.Id, Guid.NewGuid(), patchDoc);

            await CustomAssertions.AssertNotFoundAsync(response, "Item not found");
        }
    }
}