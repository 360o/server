using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace _360.Server.UnitTests.Api.V1.Stores.Model
{
    [TestClass]
    public class OfferTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnOffer()
        {
            var storeId = _faker.Random.Uuid();

            var offer = new Offer(storeId);

            Assert.AreEqual(storeId, offer.StoreId);
        }

        [TestMethod]
        public void SetEnglishName()
        {
            var offer = MakeRandomOffer();

            var englishName = _faker.Commerce.ProductAdjective();

            offer.EnglishName = englishName;

            Assert.AreEqual(englishName, offer.EnglishName);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishName()
        {
            var offer = MakeRandomOffer();

            offer.EnglishName = null;

            Assert.IsNull(offer.EnglishName);
        }

        [TestMethod]
        public void SetFrenchName()
        {
            var offer = MakeRandomOffer();

            var frenchName = _faker.Commerce.ProductAdjective();

            offer.FrenchName = frenchName;

            Assert.AreEqual(frenchName, offer.FrenchName);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchName()
        {
            var offer = MakeRandomOffer();

            offer.FrenchName = null;

            Assert.IsNull(offer.FrenchName);
        }

        [TestMethod]
        public void SetOfferItems()
        {
            var offer = MakeRandomOffer();

            var offerItems = new List<OfferItem>
            {
                MakeRandomOfferItem(),
                MakeRandomOfferItem()
            };

            offer.OfferItems = offerItems;

            CollectionAssert.AreEquivalent(offerItems.ToList(), offer.OfferItems.ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenDuplicateItemsSetOfferItemsShouldThrow()
        {
            var offer = MakeRandomOffer();

            var offerItem = MakeRandomOfferItem();
            var sameItemId = new OfferItem(offerItem.ItemId, _faker.Random.Int(1));

            var offerItems = new List<OfferItem>
            {
                offerItem,
                sameItemId
            };

            offer.OfferItems = offerItems;
        }

        [TestMethod]
        public void SetDiscount()
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount();

            offer.Discount = discount;

            Assert.AreEqual(discount, offer.Discount);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenNegativeOrZeroAmountSetDiscountShouldThrow(int amount)
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount() with { Amount = amount };

            offer.Discount = discount;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void GivenInvalidCurrencyCodeSetDiscountShouldThrow()
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount() with { CurrencyCode = (Iso4217CurrencyCode)(-1) };

            offer.Discount = discount;
        }

        private Offer MakeRandomOffer()
        {
            var storeId = _faker.Random.Uuid();

            return new Offer(storeId);
        }

        private OfferItem MakeRandomOfferItem()
        {
            var itemId = _faker.Random.Uuid();

            var quantity = _faker.Random.Int(1);

            var offerItem = new OfferItem(itemId, quantity);

            return offerItem;
        }

        private MoneyValue MakeRandomDiscount()
        {
            return new MoneyValue
            {
                Amount = _faker.Random.Decimal(0, 100),
                CurrencyCode = _faker.PickRandom<Iso4217CurrencyCode>()
            };
        }
    }
}