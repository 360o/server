using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class EnglishDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var item = Generator.MakeRandomItem();

            var englishDescription = Fakers.EnglishFaker.Commerce.ProductAdjective();

            item.EnglishDescription = englishDescription;

            Assert.AreEqual(englishDescription, item.EnglishDescription);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.EnglishDescription = null;

            Assert.IsNull(item.EnglishDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string englishName)
        {
            var item = Generator.MakeRandomItem();

            item.EnglishDescription = englishName;

            Assert.IsNull(item.EnglishDescription);
        }
    }
}