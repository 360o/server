using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Offer
{
    [TestClass]
    public class EnglishNameTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var offer = Generator.MakeRandomOffer();

            var englishName = Fakers.EnglishFaker.Commerce.ProductName();

            offer.EnglishName = englishName;

            Assert.AreEqual(englishName, offer.EnglishName);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var offer = Generator.MakeRandomOffer();

            offer.EnglishName = null;

            Assert.IsNull(offer.EnglishName);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string englishName)
        {
            var offer = Generator.MakeRandomOffer();

            offer.EnglishName = englishName;

            Assert.IsNull(offer.EnglishName);
        }
    }
}
