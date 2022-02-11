using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace _360.Server.UnitTests.Api.V1.Organizations.Model.Organization
{
    [TestClass]
    public class FrenchCategoriesTest
    {
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(20)]
        public void GivenRandomArgumentShouldSetToArgumentRemovingDuplicatesAndSetFrenchJoinedCategories(int numberOfCategories)
        {
            var categories = Fakers.FrenchFaker.Commerce.Categories(numberOfCategories).ToList();

            var categoriesSet = categories.ToHashSet();

            var joinedCategories = string.Join(" ", categoriesSet);

            var organization = Generator.MakeRandomOrganization();

            organization.FrenchCategories = categories;

            CollectionAssert.AreEquivalent(categoriesSet.ToList(), organization.FrenchCategories);
            Assert.AreEqual(joinedCategories, organization.FrenchJoinedCategories);
        }

        [TestMethod]
        public void GivenNullShouldSetToNullAndSetFrenchJoinedCategoriesToEmpty()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchCategories = null;

            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchJoinedCategories);
        }

        [TestMethod]
        public void GivenEmptyShouldSetToNullAndSetFrenchJoinedCategoriesToEmpty()
        {
            var organization = Generator.MakeRandomOrganization();

            organization.FrenchCategories = new List<string>();

            Assert.IsNull(organization.FrenchCategories);
            Assert.AreEqual(string.Empty, organization.FrenchJoinedCategories);
        }
    }
}