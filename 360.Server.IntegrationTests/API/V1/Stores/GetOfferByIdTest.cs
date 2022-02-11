using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class GetOfferByIdTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public async Task GivenOfferExistsShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var item = await ProgramTest.ApiClientUser1.Stores.CreateRandomItemAndDeserializeAsync(store.Id);

            var requestItems = new HashSet<CreateOfferRequestItem>
            {
                new CreateOfferRequestItem
                {
                    ItemId = item.Id,
                    Quantity = _faker.Random.Int(1, 10)
                }
            };

            var discount = new MoneyValue()
            {
                Amount = _faker.Random.Decimal(1, 10),
                CurrencyCode = _faker.PickRandom<Iso4217CurrencyCode>()
            };

            var createdOffer = await ProgramTest.ApiClientUser1.Stores.CreateRandomOfferAndDeserializeAsync(store.Id, requestItems, discount);

            var offer = await ProgramTest.ApiClientUser1.Stores.GetOfferByIdAndDeserializeAsync(createdOffer.StoreId, createdOffer.Id);

            StoresHelper.AssertOffersAreEqual(createdOffer, offer);
        }

        [TestMethod]
        public async Task GivenStoreDoesNotExistsShouldReturnNotFound()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.GetOfferByIdAsync(Guid.NewGuid(), Guid.NewGuid());

            await CustomAssertions.AssertNotFoundAsync(response, "Store not found");
        }

        [TestMethod]
        public async Task GivenOfferDoesNotExistsShouldReturnNotFound()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var store = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(organization.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.GetOfferByIdAsync(store.Id, Guid.NewGuid());

            await CustomAssertions.AssertNotFoundAsync(response, "Offer not found");
        }
    }
}