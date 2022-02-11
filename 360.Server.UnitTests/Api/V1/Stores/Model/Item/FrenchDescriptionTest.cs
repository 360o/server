using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class FrenchDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var item = Generator.MakeRandomItem();

            var frenchDescription = Fakers.FrenchFaker.Commerce.ProductAdjective();

            item.FrenchDescription = frenchDescription;

            Assert.AreEqual(frenchDescription, item.FrenchDescription);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.FrenchDescription = null;

            Assert.IsNull(item.FrenchDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string frenchName)
        {
            var item = Generator.MakeRandomItem();

            item.FrenchDescription = frenchName;

            Assert.IsNull(item.FrenchDescription);
        }
    }
}