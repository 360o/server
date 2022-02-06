using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360o.Server.Api.V1.Organizations.DTOs;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Organizations
{
    [TestClass]
    public class UpdateOrganizationTest
    {
        [TestMethod]
        public async Task GivenNameNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker();

            var request = new UpdateOrganizationRequest
            {
                Name = faker.Company.CompanyName()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            Assert.AreEqual(request.Name, updatedOrganization.Name);
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { Name = organization.Name });
        }

        [TestMethod]
        public async Task GivenEnglishShortDescriptionNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker();

            var request = new UpdateOrganizationRequest
            {
                EnglishShortDescription = faker.Company.CatchPhrase()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            Assert.AreEqual(request.EnglishShortDescription, updatedOrganization.EnglishShortDescription);
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { EnglishShortDescription = organization.EnglishShortDescription });
        }

        [TestMethod]
        public async Task GivenEnglishLongDescriptionNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker();

            var request = new UpdateOrganizationRequest
            {
                EnglishLongDescription = faker.Company.CatchPhrase()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            Assert.AreEqual(request.EnglishLongDescription, updatedOrganization.EnglishLongDescription);
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { EnglishLongDescription = organization.EnglishLongDescription });
        }

        [TestMethod]
        public async Task GivenEnglishCategoriesNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker();

            var request = new UpdateOrganizationRequest
            {
                EnglishCategories = faker.Commerce.Categories(faker.Random.Int(0, 5)).ToHashSet()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            CollectionAssert.AreEquivalent(request.EnglishCategories.ToList(), updatedOrganization.EnglishCategories.ToList());
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { EnglishCategories = organization.EnglishCategories });
        }

        [TestMethod]
        public async Task GivenFrenchShortDescriptionNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker("fr");

            var request = new UpdateOrganizationRequest
            {
                FrenchShortDescription = faker.Company.CatchPhrase()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            Assert.AreEqual(request.FrenchShortDescription, updatedOrganization.FrenchShortDescription);
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { FrenchShortDescription = organization.FrenchShortDescription });
        }

        [TestMethod]
        public async Task GivenFrenchLongDescriptionNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker("fr");

            var request = new UpdateOrganizationRequest
            {
                FrenchLongDescription = faker.Company.CatchPhrase()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            Assert.AreEqual(request.FrenchLongDescription, updatedOrganization.FrenchLongDescription);
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { FrenchLongDescription = organization.FrenchLongDescription });
        }

        [TestMethod]
        public async Task GivenFrenchCategoriesNotNullShouldReturnOK()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var faker = new Faker("fr");

            var request = new UpdateOrganizationRequest
            {
                FrenchCategories = faker.Commerce.Categories(faker.Random.Int(0, 5)).ToHashSet()
            };

            var updatedOrganization = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAndDeserializeAsync(organization.Id, request);

            CollectionAssert.AreEquivalent(request.FrenchCategories.ToList(), updatedOrganization.FrenchCategories.ToList());
            OrganizationsHelper.AssertOrganizationsAreEqual(organization, updatedOrganization with { FrenchCategories = organization.FrenchCategories });
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = new UpdateOrganizationRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PatchAsync(OrganizationsHelper.OrganizationRoute(Guid.NewGuid()), requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotBelongToUserShouldReturnForbidden()
        {
            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var request = new UpdateOrganizationRequest();

            var response = await ProgramTest.ApiClientUser2.Organizations.UpdateOrganizationAsync(organization.Id, request);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenOrganizationDoesNotExistShouldReturnNotFound()
        {
            var request = new UpdateOrganizationRequest();

            var response = await ProgramTest.ApiClientUser1.Organizations.UpdateOrganizationAsync(Guid.NewGuid(), request);

            await ProblemDetailAssertions.AssertNotFoundAsync(response, "Organization not found");
        }
    }
}