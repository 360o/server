using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Organizations
{
    [TestClass]
    public class PatchOrganizationTest
    {
        private readonly Faker _englishFaker = new Faker();
        private readonly Faker _frenchFaker = new Faker("fr");

        [TestMethod]
        public async Task GivenNameShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var name = _englishFaker.Company.CompanyName();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Name",
                    value = name
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.AreEqual(name, updatedOrganization.Name);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { Name = organization.Name });
        }

        [TestMethod]
        public async Task GivenNullNameShouldReturnBadRequest()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Name",
                    value = (string)null
                }
            };

            var response = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAsync(organization.Id, patchDoc);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Name");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceNameShouldReturnBadRequest(string name)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/Name",
                    value = name
                }
            };

            var response = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAsync(organization.Id, patchDoc);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Name");
        }

        [TestMethod]
        public async Task GivenEnglishShortDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var englishShortDescription = _englishFaker.Company.CatchPhrase();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishShortDescription",
                    value = englishShortDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.AreEqual(englishShortDescription, updatedOrganization.EnglishShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishShortDescription = organization.EnglishShortDescription });
        }

        [TestMethod]
        public async Task GivenNullEnglishShortDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishShortDescription",
                    value = (List<string>)null
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.EnglishShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishShortDescription = organization.EnglishShortDescription });
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceEnglishShortDescriptionShouldReturnOK(string englishShortDescription)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishShortDescription",
                    value = englishShortDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.EnglishShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishShortDescription = organization.EnglishShortDescription });
        }

        [TestMethod]
        public async Task GivenEnglishLongDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var englishLongDescription = _englishFaker.Company.CatchPhrase();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishLongDescription",
                    value = englishLongDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.AreEqual(englishLongDescription, updatedOrganization.EnglishLongDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishLongDescription = organization.EnglishLongDescription });
        }

        [TestMethod]
        public async Task GivenEnglishCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var categories = _englishFaker.Commerce.Categories(_englishFaker.Random.Int(1, 10));

            var categoriesSet = categories.ToHashSet();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishCategories",
                    value = categories
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNotNull(updatedOrganization.EnglishCategories);
            CollectionAssert.AreEquivalent(categoriesSet.ToList(), updatedOrganization.EnglishCategories.ToList());
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishCategories = organization.EnglishCategories });
        }

        [TestMethod]
        public async Task GivenNullEnglishCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishCategories",
                    value = (List<string>)null
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.EnglishCategories);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishCategories = organization.EnglishCategories });
        }

        [TestMethod]
        public async Task GivenEmptyEnglishCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/EnglishCategories",
                    value = new List<string>()
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.EnglishCategories);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { EnglishCategories = organization.EnglishCategories });
        }

        [TestMethod]
        public async Task GivenFrenchShortDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var frenchShortDescription = _frenchFaker.Company.CatchPhrase();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchShortDescription",
                    value = frenchShortDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.AreEqual(frenchShortDescription, updatedOrganization.FrenchShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchShortDescription = organization.FrenchShortDescription });
        }

        [TestMethod]
        public async Task GivenNullFrenchShortDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchShortDescription",
                    value = (List<string>)null
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchShortDescription = organization.FrenchShortDescription });
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceFrenchShortDescriptionShouldReturnOK(string frenchShortDescription)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchShortDescription",
                    value = frenchShortDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchShortDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchShortDescription = organization.FrenchShortDescription });
        }

        [TestMethod]
        public async Task GivenFrenchLongDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var frenchLongDescription = _frenchFaker.Company.CatchPhrase();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchLongDescription",
                    value = frenchLongDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.AreEqual(frenchLongDescription, updatedOrganization.FrenchLongDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchLongDescription = organization.FrenchLongDescription });
        }

        [TestMethod]
        public async Task GivenNullFrenchLongDescriptionShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchLongDescription",
                    value = (List<string>)null
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchLongDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchLongDescription = organization.FrenchLongDescription });
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceFrenchLongDescriptionShouldReturnOK(string frenchLongDescription)
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchLongDescription",
                    value = frenchLongDescription
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchLongDescription);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchLongDescription = organization.FrenchLongDescription });
        }

        public async Task GivenFrenchCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var categories = _frenchFaker.Commerce.Categories(_englishFaker.Random.Int(1, 10));

            var categoriesSet = categories.ToHashSet();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchCategories",
                    value = categories
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNotNull(updatedOrganization.FrenchCategories);
            CollectionAssert.AreEquivalent(categoriesSet.ToList(), updatedOrganization.FrenchCategories.ToList());
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchCategories = organization.FrenchCategories });
        }

        [TestMethod]
        public async Task GivenNullFrenchCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchCategories",
                    value = (List<string>)null
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchCategories);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchCategories = organization.FrenchCategories });
        }

        [TestMethod]
        public async Task GivenEmptyFrenchCategoriesShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                    op = "replace",
                    path = "/FrenchCategories",
                    value = new List<string>()
                }
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAndDeserializeAsync(organization.Id, patchDoc);

            Assert.IsNull(updatedOrganization.FrenchCategories);
            CustomAssertions.AssertSerializeToSameJson(organization, updatedOrganization with { FrenchCategories = organization.FrenchCategories });
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var patchDoc = new[]
            {
                new
                {
                }
            };

            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            var response = await ProgramTest.NewClient().PatchAsync(OrganizationsHelper.OrganizationRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser2.Organizations.PatchOrganizationAsync(organization.Id, patchDoc);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var patchDoc = new[]
            {
                new
                {
                }
            };

            var response = await ProgramTest.ApiClientUser1.Organizations.PatchOrganizationAsync(Guid.NewGuid(), patchDoc);

            await CustomAssertions.AssertNotFoundWithProblemDetailsAsync(response, "Organization not found");
        }
    }
}