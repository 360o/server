using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class EnglishNameTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var item = Generator.MakeRandomItem();

            var englishName = Fakers.EnglishFaker.Commerce.ProductName();

            item.EnglishName = englishName;

            Assert.AreEqual(englishName, item.EnglishName);
        }

        [TestMethod]
        public void GivenNullArgumentShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.EnglishName = null;

            Assert.IsNull(item.EnglishName);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentShouldSetToNull(string englishName)
        {
            var item = Generator.MakeRandomItem();

            item.EnglishName = englishName;

            Assert.IsNull(item.EnglishName);
        }
    }
}