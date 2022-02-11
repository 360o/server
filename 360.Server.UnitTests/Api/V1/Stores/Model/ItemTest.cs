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

            item.EnglishName = englishName;

            Assert.AreEqual(englishName, item.EnglishName);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishName()
        {
            var item = MakeRandomItem();

            item.EnglishName = null!;

            Assert.IsNull(item.EnglishName);
        }

        [TestMethod]
        public void SetEnglishDescription()
        {
            var item = MakeRandomItem();

            var englishDescription = _faker.Commerce.ProductDescription();

            item.EnglishDescription = englishDescription;

            Assert.AreEqual(englishDescription, item.EnglishDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishDescription()
        {
            var item = MakeRandomItem();

            item.EnglishDescription = null;

            Assert.IsNull(item.EnglishDescription);
        }

        [TestMethod]
        public void SetFrenchName()
        {
            var item = MakeRandomItem();

            var frenchName = _faker.Commerce.ProductName();

            item.FrenchName = frenchName;

            Assert.AreEqual(frenchName, item.FrenchName);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchName()
        {
            var item = MakeRandomItem();

            item.FrenchName = null;

            Assert.IsNull(item.FrenchName);
        }

        [TestMethod]
        public void SetFrenchDescription()
        {
            var item = MakeRandomItem();

            var frenchDescription = _faker.Commerce.ProductDescription();

            item.FrenchDescription = frenchDescription;

            Assert.AreEqual(frenchDescription, item.FrenchDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchDescription()
        {
            var item = MakeRandomItem();

            item.FrenchDescription = null;

            Assert.IsNull(item.FrenchDescription);
        }

        [TestMethod]
        public void SetPrice()
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice();

            item.Price = price;

            Assert.AreEqual(price, item.Price);
        }

        [TestMethod]
        public void GivenNullArgumentSetPrice()
        {
            var item = MakeRandomItem();

            item.Price = null;

            Assert.IsNull(item.Price);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenZeroOrNegativeAmountSetPriceShouldThrow(decimal amount)
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice() with { Amount = amount };

            item.Price = price;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void GivenInvalidCurrencyCodeSetPriceShouldThrow()
        {
            var item = MakeRandomItem();

            var price = MakeRandomPrice() with { CurrencyCode = (Iso4217CurrencyCode)(-1) };

            item.Price = price;
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