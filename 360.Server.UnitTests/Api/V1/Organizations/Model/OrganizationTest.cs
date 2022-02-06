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
            Assert.AreEqual(string.Empty, organization.EnglishShortDescription);
            Assert.AreEqual(string.Empty, organization.EnglishLongDescription);
            CollectionAssert.AreEqual(new List<string>(), organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.FrenchShortDescription);
            Assert.AreEqual(string.Empty, organization.FrenchLongDescription);
            CollectionAssert.AreEqual(new List<string>(), organization.FrenchCategories);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'userId')")]
        public void GivenNullUserIdShouldThrowArgumentNullException()
        {
            var name = _faker.Company.CompanyName();

            new Organization(null!, name);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException), "Required input userId was empty. (Parameter 'userId')")]
        public void GivenWhitespaceUserIdShouldThrow(string userId)
        {
            var name = _faker.Company.CompanyName();

            new Organization(userId, name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'name')")]
        public void GivenNullNameShouldThrow()
        {
            var userId = _faker.Internet.UserName();

            new Organization(userId, null!);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException), "Required input name was empty. (Parameter 'name')")]
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

            organization.SetEnglishShortDescription(englishShortDescription);

            Assert.AreEqual(englishShortDescription, organization.EnglishShortDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'englishShortDescription')")]
        public void GivenNullArgumentSetEnglishShortDescriptionShouldThrow()
        {
            var organization = MakeRandomOrganization();

            organization.SetEnglishShortDescription(null!);
        }

        [TestMethod]
        public void SetEnglishLongDescription()
        {
            var organization = MakeRandomOrganization();

            var englishLongDescription = _faker.Company.CatchPhrase();

            organization.SetEnglishLongDescription(englishLongDescription);

            Assert.AreEqual(englishLongDescription, organization.EnglishLongDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'englishLongDescription')")]
        public void GivenNullArgumentSetEnglishLongDescriptionShouldThrow()
        {
            var organization = MakeRandomOrganization();

            organization.SetEnglishLongDescription(null!);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        public void SetEnglishCategoriesShouldAlsoSetEnglishCategoriesJoined(int numberOfCategories)
        {
            var categories = _faker.Commerce.Categories(numberOfCategories).ToHashSet();

            var joinedCategories = string.Join(" ", categories);

            var organization = MakeRandomOrganization();

            organization.SetEnglishCategories(categories);

            CollectionAssert.AreEquivalent(categories.ToList(), organization.EnglishCategories);
            Assert.AreEqual(joinedCategories, organization.EnglishCategoriesJoined);
        }

        [TestMethod]
        public void SetFrenchShortDescription()
        {
            var organization = MakeRandomOrganization();

            var frenchShortDescription = _faker.Company.CatchPhrase();

            organization.SetFrenchShortDescription(frenchShortDescription);

            Assert.AreEqual(frenchShortDescription, organization.FrenchShortDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'frenchShortDescription')")]
        public void GivenNullArgumentSetFrenchShortDescriptionShouldThrow()
        {
            var organization = MakeRandomOrganization();

            organization.SetFrenchShortDescription(null!);
        }

        [TestMethod]
        public void SetFrenchLongDescription()
        {
            var organization = MakeRandomOrganization();

            var frenchLongDescription = _faker.Company.CatchPhrase();

            organization.SetFrenchLongDescription(frenchLongDescription);

            Assert.AreEqual(frenchLongDescription, organization.FrenchLongDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'frenchLongDescription')")]
        public void GivenNullArgumentSetFrenchLongDescriptionShouldThrow()
        {
            var organization = MakeRandomOrganization();

            organization.SetFrenchLongDescription(null!);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        public void SetFrenchCategoriesShouldAlsoSetFrenchCategoriesJoined(int numberOfCategories)
        {
            var categories = _faker.Commerce.Categories(numberOfCategories).ToHashSet();

            var joinedCategories = string.Join(" ", categories);

            var organization = MakeRandomOrganization();

            organization.SetFrenchCategories(categories);

            CollectionAssert.AreEquivalent(categories.ToList(), organization.FrenchCategories);
            Assert.AreEqual(joinedCategories, organization.FrenchCategoriesJoined);
        }

        private Organization MakeRandomOrganization()
        {
            var userId = _faker.Internet.UserName();
            var name = _faker.Company.CompanyName();

            return new Organization(userId, name);
        }
    }
}