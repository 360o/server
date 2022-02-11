using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Offer
{
    [TestClass]
    public class FrenchNameTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var offer = Generator.MakeRandomOffer();

            var frenchName = Fakers.FrenchFaker.Commerce.ProductName();

            offer.FrenchName = frenchName;

            Assert.AreEqual(frenchName, offer.FrenchName);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var offer = Generator.MakeRandomOffer();

            offer.FrenchName = null;

            Assert.IsNull(offer.FrenchName);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string frenchName)
        {
            var offer = Generator.MakeRandomOffer();

            offer.FrenchName = frenchName;

            Assert.IsNull(offer.FrenchName);
        }
    }
}
