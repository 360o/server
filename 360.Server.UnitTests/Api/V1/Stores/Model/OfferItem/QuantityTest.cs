using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.OfferItem
{
    [TestClass]
    public class QuantityTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var offerItem = Generator.MakeRandomOfferItem();

            var quantity = Fakers.EnglishFaker.Random.Int(min: 1);

            offerItem.Quantity = quantity;

            Assert.AreEqual(quantity, offerItem.Quantity);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenNegativeOrZeroShouldThrow(int quantity)
        {
            var offerItem = Generator.MakeRandomOfferItem();

            offerItem.Quantity = quantity;
        }
    }
}
