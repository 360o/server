using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class EnglishShortDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var organization = Generator.MakeRandomOrganization();

            var englishShortDescription = Fakers.EnglishFaker.Company.CatchPhrase();

            organization.EnglishShortDescription = englishShortDescription;

            Assert.AreEqual(englishShortDescription, organization.EnglishShortDescription);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishShortDescription = null;

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceShouldSetToNull(string englishShortDescription)
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishShortDescription = englishShortDescription;

            Assert.IsNull(organization.EnglishShortDescription);
        }
    }
}