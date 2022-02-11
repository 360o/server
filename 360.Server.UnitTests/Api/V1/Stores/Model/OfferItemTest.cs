using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _360.Server.UnitTests.Api.V1.Stores.Model
{
    [TestClass]
    public class OfferItemTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnOfferItem()
        {
            var itemId = _faker.Random.Uuid();
            var quantity = _faker.Random.Int(1, int.MaxValue);

            var offerItem = new OfferItem(itemId, quantity);

            Assert.AreEqual(itemId, offerItem.ItemId);
            Assert.AreEqual(quantity, offerItem.Quantity);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenNegativeOrZeroQuantitySetQuantityShouldThrow(int quantity)
        {
            var offerItem = MakeRandomOfferItem();

            offerItem.Quantity = quantity;
        }

        private OfferItem MakeRandomOfferItem()
        {
            var itemId = _faker.Random.Uuid();
            var quantity = _faker.Random.Int(1, int.MaxValue);

            return new OfferItem(itemId, quantity);
        }
    }
}