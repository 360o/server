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

            offer.SetEnglishName(englishName);

            Assert.AreEqual(englishName, offer.EnglishName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'englishName')")]
        public void GivenNullEnglishNameSetEnglishNameShouldThrow()
        {
            var offer = MakeRandomOffer();

            offer.SetEnglishName(null!);
        }

        [TestMethod]
        public void SetFrenchName()
        {
            var offer = MakeRandomOffer();

            var frenchName = _faker.Commerce.ProductAdjective();

            offer.SetFrenchName(frenchName);

            Assert.AreEqual(frenchName, offer.FrenchName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'frenchName')")]
        public void GivenNullFrenchNameSetFrenchNameShouldThrow()
        {
            var offer = MakeRandomOffer();

            offer.SetFrenchName(null!);
        }

        [TestMethod]
        public void SetOfferItems()
        {
            var offer = MakeRandomOffer();

            var offerItems = new HashSet<OfferItem>
            {
                MakeRandomOfferItem(),
                MakeRandomOfferItem()
            };

            offer.SetOfferItems(offerItems);

            CollectionAssert.AreEquivalent(offerItems.ToList(), offer.OfferItems.ToList());
        }

        [TestMethod]
        public void SetDiscount()
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount();

            offer.SetDiscount(discount);

            Assert.AreEqual(discount, offer.Discount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Required input Amount cannot be zero or negative. (Parameter 'Amount')")]
        public void GivenInvalidAmountSetDiscountShouldThrow()
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount() with { Amount = -1 };

            offer.SetDiscount(discount);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException), "The value of argument 'CurrencyCode' (-1) is invalid for Enum type 'Iso4217CurrencyCode'. (Parameter 'CurrencyCode')")]
        public void GivenInvalidCurrencyCodeSetDiscountShouldThrow()
        {
            var offer = MakeRandomOffer();

            var discount = MakeRandomDiscount() with { CurrencyCode = (Iso4217CurrencyCode)(-1) };

            offer.SetDiscount(discount);
        }

        private Offer MakeRandomOffer()
        {
            var storeId = _faker.Random.Uuid();

            return new Offer(storeId);
        }

        private OfferItem MakeRandomOfferItem()
        {
            var itemId = _faker.Random.Uuid();

            var offerItem = new OfferItem(itemId);

            var quantity = _faker.Random.Int(1);

            offerItem.SetQuantity(quantity);

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