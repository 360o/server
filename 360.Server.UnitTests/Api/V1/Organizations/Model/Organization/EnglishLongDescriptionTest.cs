using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class EnglishLongDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var organization = Generator.MakeRandomOrganization();

            var englishLongDescription = Fakers.EnglishFaker.Company.CatchPhrase();

            organization.EnglishLongDescription = englishLongDescription;

            Assert.AreEqual(englishLongDescription, organization.EnglishLongDescription);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishLongDescription = null;

            Assert.IsNull(organization.EnglishLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceShouldSetToNull(string englishLongDescription)
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishShortDescription = englishLongDescription;

            Assert.IsNull(organization.EnglishShortDescription);
        }
    }
}