using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class CreateOfferTest
    {
        private readonly Faker _englishFaker = new Faker();

        [TestMethod]
        public async Task GivenItemsExistShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);
            var item2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId = item1.Id,
                    Quantity = _englishFaker.Random.Int(1, 10),
                },
                new CreateOfferRequestItem
                {
                    ItemId = item2.Id,
                    Quantity = _englishFaker.Random.Int(1, 10),
                },
            };

            var requestItemsDict = requestItems.ToDictionary(i => i.ItemId, i => i);

            var discount = new MoneyValueDTO
            {
                Amount = _englishFaker.Random.Decimal(1, 10),
                CurrencyCode = _englishFaker.PickRandom<Iso4217CurrencyCode>()
            };

            var offer = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, requestItems, discount);

            var offerItemsDict = offer.OfferItems.ToDictionary(i => i.ItemId, i => i);

            Assert.AreEqual(requestItemsDict.Count, offerItemsDict.Count);
            Assert.IsTrue(Guid.TryParse(offerItemsDict[item1.Id].Id.ToString(), out var _));
            Assert.AreEqual(requestItemsDict[item1.Id].Quantity, offerItemsDict[item1.Id].Quantity);
            Assert.IsTrue(Guid.TryParse(offerItemsDict[item2.Id].Id.ToString(), out var _));
            Assert.AreEqual(requestItemsDict[item2.Id].Quantity, offerItemsDict[item2.Id].Quantity);

            Assert.AreEqual(discount, offer.Discount);
        }

        [TestMethod]
        public async Task GivenDiscountNullShouldReturnCreated()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);
            var item2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId = item1.Id,
                    Quantity = _englishFaker.Random.Int(1, 10),
                },
                new CreateOfferRequestItem
                {
                    ItemId = item2.Id,
                    Quantity = _englishFaker.Random.Int(1, 10),
                },
            };

            var offer = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, requestItems, null);

            Assert.IsNull(offer.Discount);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenAllNamesNullOrWhitespaceShouldReturnBadRequest(string? nullOrWhitespaceName)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId= item1.Id,
                    Quantity= _englishFaker.Random.Int(1, 10)
                }
            };

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null) with
            {
                EnglishName = nullOrWhitespaceName,
                FrenchName = nullOrWhitespaceName,
            };

            var response = await ProgramTest.ApiClientUser1.Stores.CreateOfferAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "At least one Name must be defined");
        }

        [TestMethod]
        public async Task GivenEmptyOfferItemsShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>();

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null);

            var response = await ProgramTest.ApiClientUser1.Stores.CreateOfferAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "OfferItems");
        }

        [TestMethod]
        public async Task GivenOfferItemWithoutIdShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId= Guid.Empty,
                    Quantity= _englishFaker.Random.Int(1, 10)
                }
            };

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null);

            var response = await ProgramTest.ApiClientUser1.Stores.CreateOfferAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Item Id' must not be empty.");
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task GivenOfferItemWithZeroOrNegativeQuantityShouldReturnBadRequest(int quantity)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId= Guid.NewGuid(),
                    Quantity= quantity
                }
            };

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null);

            var response = await ProgramTest.ApiClientUser1.Stores.CreateOfferAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Quantity' must be greater than '0'");
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task GivenZeroOrNegativeDiscountAmountShouldReturnBadRequest(int amount)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>();

            var discount = new MoneyValueDTO
            {
                Amount = amount,
                CurrencyCode = _englishFaker.PickRandom<Iso4217CurrencyCode>()
            };

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, discount);

            var response = await ProgramTest.ApiClientUser1.Stores.CreateOfferAsync(store.Id, request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "OfferItems");
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnNotAuthorized()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>();

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null);

            var requestContent = JsonUtils.MakeJsonStringContent(request);

            var response = await ProgramTest.NewClient().PostAsync(StoresHelper.OffersRoute(store.Id), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId= item1.Id,
                    Quantity= _englishFaker.Random.Int(1, 10)
                }
            };

            var request = Generator.MakeRandomCreateOfferRequest(requestItems, null);

            var response = await ProgramTest.ApiClientUser2.Stores.CreateOfferAsync(store.Id, request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}