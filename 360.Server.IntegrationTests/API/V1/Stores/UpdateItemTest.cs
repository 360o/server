using _360.Server.IntegrationTests.API.V1.Helpers;
using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class UpdateItemTest
    {
        [TestMethod]
        public async Task GivenEnglishNameIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker();

            var request = new UpdateItemRequest
            {
                EnglishName = faker.Commerce.ProductName()
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.UpdateItemAndDeserializeAsync(store.Id, item.Id, request);

            Assert.AreEqual(request.EnglishName, updatedItem.EnglishName);
            Assert.AreEqual(item, updatedItem with { EnglishName = item.EnglishName });
        }

        [TestMethod]
        public async Task GivenEnglishDescriptionIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker();

            var request = new UpdateItemRequest
            {
                EnglishDescription = faker.Commerce.ProductDescription()
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.UpdateItemAndDeserializeAsync(store.Id, item.Id, request);

            Assert.AreEqual(request.EnglishDescription, updatedItem.EnglishDescription);
            Assert.AreEqual(item, updatedItem with { EnglishDescription = item.EnglishDescription });
        }

        [TestMethod]
        public async Task GivenFrenchNameIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker("fr");

            var request = new UpdateItemRequest
            {
                FrenchName = faker.Commerce.ProductName()
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.UpdateItemAndDeserializeAsync(store.Id, item.Id, request);

            Assert.AreEqual(request.FrenchName, updatedItem.FrenchName);
            Assert.AreEqual(item, updatedItem with { FrenchName = item.FrenchName });
        }

        [TestMethod]
        public async Task GivenFrenchDescriptionIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker("fr");

            var request = new UpdateItemRequest
            {
                FrenchDescription = faker.Commerce.ProductDescription()
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.UpdateItemAndDeserializeAsync(store.Id, item.Id, request);

            Assert.AreEqual(request.FrenchDescription, updatedItem.FrenchDescription);
            Assert.AreEqual(item, updatedItem with { FrenchDescription = item.FrenchDescription });
        }

        [TestMethod]
        public async Task GivenPriceIsNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker();

            var request = new UpdateItemRequest
            {
                Price = new MoneyValue
                {
                    Amount = faker.Random.Decimal(0, 100),
                    CurrencyCode = faker.PickRandom<Iso4217CurrencyCode>()
                }
            };

            var updatedItem = await ProgramTest.ApiClientUser1.Stores.UpdateItemAndDeserializeAsync(store.Id, item.Id, request);

            Assert.AreEqual(request.Price, updatedItem.Price);
            Assert.AreEqual(item, updatedItem with { Price = item.Price });
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = new UpdateStoreRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PatchAsync(StoresHelper.ItemRoute(Guid.NewGuid(), Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenItemDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var request = new UpdateItemRequest();

            var response = await ProgramTest.ApiClientUser2.Stores.UpdateItemAsync(store.Id, item.Id, request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenItemDoesNotExistShouldReturnNotFound()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = new UpdateItemRequest();

            var response = await ProgramTest.ApiClientUser1.Stores.UpdateItemAsync(store.Id, Guid.NewGuid(), request);

            await ProblemDetailAssertions.AssertNotFoundAsync(response, "Item not found");
        }

        [TestMethod]
        public async Task GivenInvalidPriceShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var faker = new Faker();

            var request = new UpdateItemRequest
            {
                Price = new MoneyValue
                {
                    Amount = faker.Random.Decimal(-1, -0.1m),
                    CurrencyCode = faker.PickRandom<Iso4217CurrencyCode>()
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.UpdateItemAsync(store.Id, item.Id, request);

            await ProblemDetailAssertions.AssertBadRequestAsync(response, "Amount");
        }
    }
}