using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class EnglishCategoriesTest
    {
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(20)]
        public void GivenRandomArgumentShouldSetToArgumentRemovingDuplicatesAndSetEnglishJoinedCategories(int numberOfCategories)
        {
            var categories = Fakers.EnglishFaker.Commerce.Categories(numberOfCategories).ToList();

            var categoriesSet = categories.ToHashSet();

            var joinedCategories = string.Join(" ", categoriesSet);

            var organization = Generator.MakeRandomOrganization();

            organization.EnglishCategories = categories;

            CollectionAssert.AreEquivalent(categoriesSet.ToList(), organization.EnglishCategories);
            Assert.AreEqual(joinedCategories, organization.EnglishJoinedCategories);
        }

        [TestMethod]
        public void GivenNullShouldSetToNullAndSetEnglishJoinedCategoriesToEmpty()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishCategories = null;

            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishJoinedCategories);
        }

        [TestMethod]
        public void GivenEmptyShouldSetToNullAndSetEnglishJoinedCategoriesToEmpty()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.EnglishCategories = new List<string>();

            Assert.IsNull(organization.EnglishCategories);
            Assert.AreEqual(string.Empty, organization.EnglishJoinedCategories);
        }
    }
}