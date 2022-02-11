using _360o.Server.Api.V1.Stores.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyValueModel = _360o.Server.Api.V1.Stores.Model.MoneyValue;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Offer
{
    [TestClass]
    public class DiscountTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var offer = Generator.MakeRandomOffer();

            var discount = Generator.MakeRandomMoneyValue();

            offer.Discount = discount;

            Assert.AreEqual(discount, offer.Discount);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var offer = Generator.MakeRandomOffer();

            offer.Discount = null;

            Assert.IsNull(offer.Discount);
        }

        [TestMethod]
        public void GivenZeroShouldSetToNull()
        {
            var offer = Generator.MakeRandomOffer();

            offer.Discount = new MoneyValueModel(
                0,
                Fakers.EnglishFaker.PickRandom<Iso4217CurrencyCode>());

            Assert.IsNull(offer.Discount);
        }
    }
}
