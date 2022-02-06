using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace _360.Server.UnitTests.Api.V1.Stores.Model
{
    [TestClass]
    public class ItemTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnItem()
        {
            var storeId = _faker.Random.Uuid();

            var item = new Item(storeId);

            Assert.AreEqual(storeId, item.StoreId);
        }

        [TestMethod]
        public void SetEnglishName()
        {
            var item = MakeRandomItem();

            var englishName = _faker.Commerce.ProductName();

            item.SetEnglishName(englishName);

            Assert.AreEqual(englishName, item.EnglishName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'englishName')")]
        public void GivenNullEnglishNameSetEnglishNameShouldThrow()
        {
            var item = MakeRandomItem();

            item.SetEnglishName(null!);
        }

        [TestMethod]
        public void SetEnglishDescription()
        {
            var item = MakeRandomItem();

            var englishDescription = _faker.Commerce.ProductDescription();

            item.SetEnglishDescription(englishDescription);

            Assert.AreEqual(englishDescription, item.EnglishDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'englishDescription')")]
        public void GivenNullEnglishDescriptionSetEnglishDescriptionShouldThrow()
        {
            var item = MakeRandomItem();

            item.SetEnglishDescription(null!);
        }

        [TestMethod]
        public void SetFrenchName()
        {
            var item = MakeRandomItem();

            var frenchName = _faker.Commerce.ProductName();

            item.SetFrenchName(frenchName);

            Assert.AreEqual(frenchName, item.FrenchName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'frenchName')")]
        public void GivenNullFrenchNameSetFrenchNameShouldThrow()
        {
            var item = MakeRandomItem();

            item.SetFrenchName(null!);
        }

        [TestMethod]
        public void SetFrenchDescription()
        {
            var item = MakeRandomItem();

            var frenchDescription = _faker.Commerce.ProductDescription();

            item.SetFrenchDescription(frenchDescription);

            Assert.AreEqual(frenchDescription, item.FrenchDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'frenchDescription')")]
        public void GivenNullFrenchDescriptionSetFrenchDescriptionShouldThrow()
        {
            var item = MakeRandomItem();

            item.SetFrenchDescription(null!);
        }

        [TestMethod]
        public void SetPrice()
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice();

            item.SetPrice(price);

            Assert.AreEqual(price, item.Price);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException), "Required input Amount cannot be zero or negative. (Parameter 'Amount')")]
        public void GivenZeroOrNegativeAmountSetPriceShouldThrow(decimal amount)
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice() with { Amount = amount };

            item.SetPrice(price);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException), "The value of argument 'CurrencyCode' (-1) is invalid for Enum type 'Iso4217CurrencyCode'. (Parameter 'CurrencyCode')")]
        public void GivenInvalidCurrencyCodeSetPriceShouldThrow()
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice() with { CurrencyCode = (Iso4217CurrencyCode)(-1) };

            item.SetPrice(price);
        }

        private Item MakeRandomItem()
        {
            var storeId = _faker.Random.Uuid();

            return new Item(storeId);
        }

        private MoneyValue MakeRandomPrice()
        {
            return new MoneyValue
            {
                Amount = _faker.Random.Decimal(0, 100),
                CurrencyCode = _faker.PickRandom<Iso4217CurrencyCode>()
            };
        }
    }
}