using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Organizations
{
    [TestClass]
    public class CreateOrganizationTest
    {
        [TestMethod]
        public async Task GivenValidRequestShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(request.Name, organization.Name);
            Assert.AreEqual(request.EnglishShortDescription, organization.EnglishShortDescription);
            Assert.AreEqual(request.EnglishLongDescription, organization.EnglishLongDescription);
            Assert.IsNotNull(request.EnglishCategories);
            Assert.IsNotNull(organization.EnglishCategories);
            CollectionAssert.AreEquivalent(request.EnglishCategories.ToList(), organization.EnglishCategories.ToList());
            Assert.AreEqual(request.FrenchShortDescription, organization.FrenchShortDescription);
            Assert.AreEqual(request.FrenchLongDescription, organization.FrenchLongDescription);
            Assert.IsNotNull(request.FrenchCategories);
            Assert.IsNotNull(organization.FrenchCategories);
            CollectionAssert.AreEquivalent(request.FrenchCategories.ToList(), organization.FrenchCategories.ToList());
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(OrganizationsHelper.OrganizationsRoute, requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public async Task GivenNullNameShouldReturnBadRequest()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { Name = null };

            var response = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Name");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceNameShouldReturnBadRequest(string name)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { Name = name };

            var response = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAsync(request);

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "Name");
        }

        [TestMethod]
        public async Task GivenNullEnglishShortDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishShortDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceEnglishShortDescriptionShouldReturnCreated(string englishShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishShortDescription = englishShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishShortDescription);
        }

        [TestMethod]
        public async Task GivenNullEnglishLongDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishLongDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceEnglishLongDescriptionShouldReturnCreated(string englishLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishLongDescription = englishLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishLongDescription);
        }

        [TestMethod]
        public async Task GivenNullEnglishCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishCategories = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishCategories);
        }

        [TestMethod]
        public async Task GivenEmptyEnglishCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishCategories = new HashSet<string>() };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.EnglishCategories);
        }

        [TestMethod]
        public async Task GivenNullFrenchShortDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchShortDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceFrenchShortDescriptionShouldReturnCreated(string frenchShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchShortDescription = frenchShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchShortDescription);
        }

        [TestMethod]
        public async Task GivenNullFrenchLongDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchLongDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenWhitespaceFrenchLongDescriptionShouldReturnCreated(string frenchLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchLongDescription = frenchLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchLongDescription);
        }

        [TestMethod]
        public async Task GivenNullFrenchCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchCategories = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchCategories);
        }

        [TestMethod]
        public async Task GivenEmptyFrenchCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchCategories = new HashSet<string>() };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.IsNull(organization.FrenchCategories);
        }
    }
}