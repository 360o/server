using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class ConstructorTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnOrganization()
        {
            var userId = _faker.Internet.UserName();
            var name = _faker.Company.CompanyName();

            var organization = new _360o.Server.Api.V1.Organizations.Model.Organization(userId, name);

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
        public void GivenNullUserIdShouldThrowArgumentNullException()
        {
            var name = _faker.Company.CompanyName();

            new _360o.Server.Api.V1.Organizations.Model.Organization(null!, name);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceUserIdShouldThrow(string userId)
        {
            var name = _faker.Company.CompanyName();

            new _360o.Server.Api.V1.Organizations.Model.Organization(userId, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullNameShouldThrow()
        {
            var userId = _faker.Internet.UserName();

            new _360o.Server.Api.V1.Organizations.Model.Organization(userId, null!);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceNameShouldThrow(string name)
        {
            var userId = _faker.Internet.UserName();

            new _360o.Server.Api.V1.Organizations.Model.Organization(userId, name);
        }
    }
}