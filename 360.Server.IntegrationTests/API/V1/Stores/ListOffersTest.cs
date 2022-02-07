using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class ListOffersTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public async Task ShouldReturnAllOffersFromStore()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);
            var anotherStore = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var storeOffer1Item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);
            var storeOffer2item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);
            var anotherStoreOffer1Item1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(anotherStore.Id);

            var storeOffer1RequestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem(storeOffer1Item1.Id, _faker.Random.Int(1, 10)),
            };

            var storeOffer1Discount = new MoneyValue
            {
                Amount = _faker.Random.Decimal(1, 10),
                CurrencyCode = _faker.PickRandom<Iso4217CurrencyCode>()
            };

            var storeOffer2RequestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem(storeOffer2item1.Id, _faker.Random.Int(1, 10)),
            };

            var anotherStoreOffer1RequestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem(anotherStoreOffer1Item1.Id, _faker.Random.Int(1, 10)),
            };

            var storeOffer1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, storeOffer1RequestItems, storeOffer1Discount);
            var storeOffer2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, storeOffer2RequestItems, null);
            var anotherStoreOffer1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(anotherStore.Id, anotherStoreOffer1RequestItems, null);

            var expectedOffers = new Dictionary<Guid, OfferDTO>
            {
                { storeOffer1.Id, storeOffer1 },
                { storeOffer2.Id, storeOffer2 }
            };

            var offers = await ProgramTest.ApiClientUser1.Stores.ListOffersAndDeserializeAsync(store.Id);

            var offersDict = offers.ToDictionary(o => o.Id, o => o);

            Assert.AreEqual(expectedOffers.Count, offersDict.Count);
            StoresHelper.AssertOffersAreEqual(storeOffer1, offersDict[storeOffer1.Id]);
            StoresHelper.AssertOffersAreEqual(storeOffer2, offersDict[storeOffer2.Id]);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListOffersAsync(Guid.NewGuid());

            await ProblemDetailAssertions.AssertNotFoundAsync(response, "Store not found");
        }
    }
}