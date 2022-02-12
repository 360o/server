using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Stores.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class CreateItemTest
    {
        [TestMethod]
        public async Task GivenStoreExistsAndValidRequestShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.AreEqual(request.EnglishName, item.EnglishName);
            Assert.AreEqual(request.EnglishDescription, item.EnglishDescription);
            Assert.AreEqual(request.FrenchName, item.FrenchName);
            Assert.AreEqual(request.FrenchDescription, item.FrenchDescription);
            Assert.AreEqual(request.Price, item.Price);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEnglishNameIsNullOrWhitespaceShouldReturnCreated(string englishName)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with { EnglishName = englishName };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.EnglishName);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenFrenchNameIsNullOrWhitespaceShouldReturnCreated(string frenchName)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with { FrenchName = frenchName };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.FrenchName);
        }

        [TestMethod]
        public async Task GivenAllNamesAreNullShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with
            {
                EnglishName = null,
                FrenchName = null,
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateItemAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "At least one Name must be defined");
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = Generator.MakeRandomCreateItemRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(StoresHelper.ItemsRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceEnglishDescriptionShouldReturnCreated(string englishDescription)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with { EnglishDescription = englishDescription };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.EnglishDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFrenchDescriptionShouldReturnCreated(string frenchDescription)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with { FrenchDescription = frenchDescription };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.FrenchDescription);
        }

        [TestMethod]
        public async Task GivenNullPriceShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with { Price = null };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.Price);
        }

        [TestMethod]
        public async Task GivenZeroPriceShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with
            {
                Price = Generator.MakeRandomMoneyValueGreaterThanZero() with
                {
                    Amount = 0
                }
            };

            var item = await ProgramTest.ApiClientUser1.Stores.CreateItemAndDeserializeAsync(store.Id, request);

            Assert.IsNull(item.Price);
        }

        [TestMethod]
        public async Task GivenNegativePriceShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            request = request with
            {
                Price = Generator.MakeRandomMoneyValueGreaterThanZero() with
                {
                    Amount = -1
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateItemAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Amount' must be greater than or equal to '0'");
        }

        [TestMethod]
        public async Task GivenInvalidCurrencyCodeShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var request = Generator.MakeRandomCreateItemRequest();

            var invalidCurrencyCode = -1;

            request = request with
            {
                Price = Generator.MakeRandomMoneyValueGreaterThanZero() with
                {
                    CurrencyCode = (Iso4217CurrencyCode)(invalidCurrencyCode)
                }
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateItemAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, $"'Currency Code' has a range of values which does not include '{invalidCurrencyCode}'");
        }
    }
}