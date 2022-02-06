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

            var offerItem = new OfferItem(itemId);

            Assert.AreEqual(itemId, offerItem.ItemId);
            Assert.IsNull(offerItem.Quantity);
        }

        [TestMethod]
        public void SetQuantity()
        {
            var offerItem = MakeRandomOfferItem();

            var quantity = _faker.Random.Int(1);

            offerItem.SetQuantity(quantity);

            Assert.AreEqual(quantity, offerItem.Quantity);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException), "Required input quantity cannot be zero or negative. (Parameter 'quantity')")]
        public void GivenNegativeOrZeroQuantitySetQuantityShouldThrow(int quantity)
        {
            var offerItem = MakeRandomOfferItem();

            offerItem.SetQuantity(quantity);
        }

        private OfferItem MakeRandomOfferItem()
        {
            var itemId = _faker.Random.Uuid();

            return new OfferItem(itemId);
        }
    }
}