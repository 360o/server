using _360o.Server.Api.V1.Stores.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using MoneyValueModel = _360o.Server.Api.V1.Stores.Model.MoneyValue;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.MoneyValue
{
    [TestClass]
    public class MoneyValueConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnMoneyValue()
        {
            var amount = Fakers.EnglishFaker.Random.Decimal();
            var currencyCode = Fakers.EnglishFaker.PickRandom<Iso4217CurrencyCode>();

            var moneyValue = new MoneyValueModel(amount, currencyCode);

            Assert.AreEqual(amount, moneyValue.Amount);
            Assert.AreEqual(currencyCode, moneyValue.CurrencyCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenNegativeAmountShouldThrow()
        {
            var amount = -1;
            var currencyCode = Fakers.EnglishFaker.PickRandom<Iso4217CurrencyCode>();

            new MoneyValueModel(amount, currencyCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void GivenInvalidCurrencyCodeShouldThrow()
        {
            var amount = Fakers.EnglishFaker.Random.Decimal();
            var currencyCode = (Iso4217CurrencyCode)(-1);

            new MoneyValueModel(amount, currencyCode);
        }
    }
}