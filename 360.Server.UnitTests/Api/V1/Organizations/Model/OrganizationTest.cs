using _360o.Server.Api.V1.Organizations.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model
{
    [TestClass]
    public class OrganizationTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnOrganization()
        {
            var userId = _faker.Internet.UserName();
            var name = _faker.Company.CompanyName();

            var organization = new Organization(userId, name);

            Assert.AreEqual(userId, organization.UserId);
            Assert.AreEqual(name, organization.Name);
            Assert.IsNull(organization.EnglishShortDescription);
            Assert.IsNull(organization.EnglishLongDescription);
            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishCategoriesJoined);
            Assert.IsNull(organization.FrenchShortDescription);
            Assert.IsNull(organization.FrenchLongDescription);
            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchCategoriesJoined);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullUserIdShouldThrowArgumentNullException()
        {
            var name = _faker.Company.CompanyName();

            new Organization(null!, name);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceUserIdShouldThrow(string userId)
        {
            var name = _faker.Company.CompanyName();

            new Organization(userId, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullNameShouldThrow()
        {
            var userId = _faker.Internet.UserName();

            new Organization(userId, null!);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceNameShouldThrow(string name)
        {
            var userId = _faker.Internet.UserName();

            new Organization(userId, name);
        }

        [TestMethod]
        public void SetEnglishShortDescription()
        {
            var organization = MakeRandomOrganization();

            var englishShortDescription = _faker.Company.CatchPhrase();

            organization.EnglishShortDescription = englishShortDescription;

            Assert.AreEqual(englishShortDescription, organization.EnglishShortDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishShortDescription()
        {
            var organization = MakeRandomOrganization();

            organization.EnglishShortDescription = null;

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentSetEnglishShortDescription(string englishShortDescription)
        {
            var organization = MakeRandomOrganization();

            organization.EnglishShortDescription = englishShortDescription;

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [TestMethod]
        public void SetEnglishLongDescription()
        {
            var organization = MakeRandomOrganization();

            var englishLongDescription = _faker.Company.CatchPhrase();

            organization.EnglishLongDescription = englishLongDescription;

            Assert.AreEqual(englishLongDescription, organization.EnglishLongDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishLongDescription()
        {
            var organization = MakeRandomOrganization();

            organization.EnglishLongDescription = null;

            Assert.IsNull(organization.EnglishLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentSetEnglishLongDescription(string englishLongDescription)
        {
            var organization = MakeRandomOrganization();

            organization.EnglishShortDescription = englishLongDescription;

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void SetEnglishCategoriesShouldRemoveDuplicatesAndSetEnglishCategoriesJoined(int numberOfCategories)
        {
            var categories = _faker.Commerce.Categories(numberOfCategories).ToList();

            var categoriesSet = categories.ToHashSet();

            var joinedCategories = string.Join(" ", categoriesSet);

            var organization = MakeRandomOrganization();

            organization.EnglishCategories = categories;

            CollectionAssert.AreEquivalent(categoriesSet.ToList(), organization.EnglishCategories);
            Assert.AreEqual(joinedCategories, organization.EnglishCategoriesJoined);
        }

        [TestMethod]
        public void GivenNullArgumentSetEnglishCategories()
        {
            var organization = MakeRandomOrganization();

            organization.EnglishCategories = null;

            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishCategoriesJoined);
        }

        [TestMethod]
        public void GivenEmptyArgumentSetEnglishCategories()
        {
            var organization = MakeRandomOrganization();

            organization.EnglishCategories = new List<string>();

            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishCategoriesJoined);
        }

        [TestMethod]
        public void SetFrenchShortDescription()
        {
            var organization = MakeRandomOrganization();

            var frenchShortDescription = _faker.Company.CatchPhrase();

            organization.FrenchShortDescription = frenchShortDescription;

            Assert.AreEqual(frenchShortDescription, organization.FrenchShortDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchShortDescription()
        {
            var organization = MakeRandomOrganization();

            organization.FrenchShortDescription = null;

            Assert.IsNull(organization.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentSetFrenchShortDescription(string frenchShortDescription)
        {
            var organization = MakeRandomOrganization();

            organization.FrenchShortDescription = frenchShortDescription;

            Assert.IsNull(organization.FrenchShortDescription);
        }

        [TestMethod]
        public void SetFrenchLongDescription()
        {
            var organization = MakeRandomOrganization();

            var frenchLongDescription = _faker.Company.CatchPhrase();

            organization.FrenchLongDescription = frenchLongDescription;

            Assert.AreEqual(frenchLongDescription, organization.FrenchLongDescription);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchLongDescription()
        {
            var organization = MakeRandomOrganization();

            organization.FrenchLongDescription = null;

            Assert.IsNull(organization.FrenchLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void GivenWhitespaceArgumentSetFrenchLongDescription(string frenchLongDescription)
        {
            var organization = MakeRandomOrganization();

            organization.FrenchLongDescription = frenchLongDescription;

            Assert.IsNull(organization.FrenchLongDescription);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void SetFrenchCategoriesShouldRemoveDuplicatesAndSetFrenchCategoriesJoined(int numberOfCategories)
        {
            var categories = _faker.Commerce.Categories(numberOfCategories).ToList();

            var categoriesSet = categories.ToHashSet();

            var joinedCategories = string.Join(" ", categoriesSet);

            var organization = MakeRandomOrganization();

            organization.FrenchCategories = categories;

            CollectionAssert.AreEquivalent(categoriesSet.ToList(), organization.FrenchCategories);
            Assert.AreEqual(joinedCategories, organization.FrenchCategoriesJoined);
        }

        [TestMethod]
        public void GivenNullArgumentSetFrenchCategories()
        {
            var organization = MakeRandomOrganization();

            organization.FrenchCategories = null;

            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchCategoriesJoined);
        }

        [TestMethod]
        public void GivenEmptyArgumentSetFrenchCategories()
        {
            var organization = MakeRandomOrganization();

            organization.FrenchCategories = new List<string>();

            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchCategoriesJoined);
        }

        private Organization MakeRandomOrganization()
        {
            var userId = _faker.Internet.UserName();
            var name = _faker.Company.CompanyName();

            return new Organization(userId, name);
        }
    }
}