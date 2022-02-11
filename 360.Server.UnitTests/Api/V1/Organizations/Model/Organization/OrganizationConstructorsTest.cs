using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OrganizationModel = _360o.Server.Api.V1.Organizations.Model.Organization;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class OrganizationConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnOrganization()
        {
            var userId = Fakers.EnglishFaker.Internet.UserName();
            var name = Fakers.EnglishFaker.Company.CompanyName();

            var organization = new OrganizationModel(userId, name);

            Assert.AreEqual(userId, organization.UserId);
            Assert.AreEqual(name, organization.Name);
            Assert.IsNull(organization.EnglishShortDescription);
            Assert.IsNull(organization.EnglishLongDescription);
            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishJoinedCategories);
            Assert.IsNull(organization.FrenchShortDescription);
            Assert.IsNull(organization.FrenchLongDescription);
            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchJoinedCategories);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullUserIdShouldThrow()
        {
            var name = Fakers.EnglishFaker.Company.CompanyName();

            new OrganizationModel(null!, name);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceUserIdShouldThrow(string userId)
        {
            var name = Fakers.EnglishFaker.Company.CompanyName();

            new OrganizationModel(userId, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullNameShouldThrow()
        {
            var userId = Fakers.EnglishFaker.Internet.UserName();

            new OrganizationModel(userId, null!);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceNameShouldThrow(string name)
        {
            var userId = Fakers.EnglishFaker.Internet.UserName();

            new OrganizationModel(userId, name);
        }
    }
}