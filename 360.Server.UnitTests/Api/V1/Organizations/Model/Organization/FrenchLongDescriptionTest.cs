using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class FrenchLongDescriptionTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var organization = Generator.MakeRandomOrganization();

            var frenchLongDescription = Fakers.FrenchFaker.Company.CatchPhrase();

            organization.FrenchLongDescription = frenchLongDescription;

            Assert.AreEqual(frenchLongDescription, organization.FrenchLongDescription);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchLongDescription = null;

            Assert.IsNull(organization.FrenchLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceShouldSetToNull(string frenchLongDescription)
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchLongDescription = frenchLongDescription;

            Assert.IsNull(organization.FrenchLongDescription);
        }
    }
}