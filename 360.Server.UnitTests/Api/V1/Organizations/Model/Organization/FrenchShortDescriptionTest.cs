using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class FrenchShortDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var organization = Generator.MakeRandomOrganization();

            var frenchShortDescription = Fakers.FrenchFaker.Company.CatchPhrase();

            organization.FrenchShortDescription = frenchShortDescription;

            Assert.AreEqual(frenchShortDescription, organization.FrenchShortDescription);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchShortDescription = null;

            Assert.IsNull(organization.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceShouldSetToNull(string frenchShortDescription)
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchShortDescription = frenchShortDescription;

            Assert.IsNull(organization.FrenchShortDescription);
        }
    }
}