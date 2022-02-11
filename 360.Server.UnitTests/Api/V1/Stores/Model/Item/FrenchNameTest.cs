using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class FrenchNameTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var item = Generator.MakeRandomItem();

            var frenchName = Fakers.FrenchFaker.Commerce.ProductName();

            item.FrenchName = frenchName;

            Assert.AreEqual(frenchName, item.FrenchName);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.FrenchName = null;

            Assert.IsNull(item.FrenchName);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string frenchName)
        {
            var item = Generator.MakeRandomItem();

            item.FrenchName = frenchName;

            Assert.IsNull(item.FrenchName);
        }
    }
}