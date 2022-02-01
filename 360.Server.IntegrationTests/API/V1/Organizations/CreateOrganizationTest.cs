using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using _360o.Server.API.V1.Errors.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Organizations
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
            CollectionAssert.AreEquivalent(request.EnglishCategories?.ToList(), organization.EnglishCategories.ToList());
            Assert.AreEqual(request.FrenchShortDescription, organization.FrenchShortDescription);
            Assert.AreEqual(request.FrenchLongDescription, organization.FrenchLongDescription);
            CollectionAssert.AreEquivalent(request.FrenchCategories?.ToList(), organization.FrenchCategories.ToList());
        }

        [TestMethod]
        public async Task GivenNoAccessTokenShouldReturnUnauthorized()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient().PostAsync(OrganizationsHelper.OrganizationsRoute, requestContent);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GivenNullNameShouldReturnBadRequest()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { Name = null };

            var response = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyOrWhitespaceNameShouldReturnBadRequest(string name)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { Name = name };

            var response = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAsync(request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Name"));
        }

        [TestMethod]
        public async Task GivenNullEnglishShortDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishShortDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyOrWhitespaceEnglishShortDescriptionShouldReturnCreated(string englishShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishShortDescription = englishShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(englishShortDescription, organization.EnglishShortDescription);
        }

        [TestMethod]
        public async Task GivenNullEnglishLongDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishLongDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.EnglishLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyOrWhitespaceEnglishLongDescriptionShouldReturnCreated(string englishLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishLongDescription = englishLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(englishLongDescription, organization.EnglishLongDescription);
        }

        [TestMethod]
        public async Task GivenNullEnglishCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishCategories = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(0, organization.EnglishCategories.Count);
        }

        [TestMethod]
        public async Task GivenEmptyEnglishCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishCategories = new HashSet<string>() };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(0, organization.EnglishCategories.Count);
        }

        [TestMethod]
        public async Task GivenNullFrenchShortDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchShortDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyOrWhitespaceFrenchShortDescriptionShouldReturnCreated(string frenchShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchShortDescription = frenchShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(frenchShortDescription, organization.FrenchShortDescription);
        }

        [TestMethod]
        public async Task GivenNullFrenchLongDescriptionShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchLongDescription = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.FrenchLongDescription);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyhitespaceFrenchLongDescriptionShouldReturnCreated(string frenchLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchLongDescription = frenchLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(frenchLongDescription, organization.FrenchLongDescription);
        }

        [TestMethod]
        public async Task GivenNullFrenchCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchCategories = null };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(0, organization.FrenchCategories.Count);
        }

        [TestMethod]
        public async Task GivenEmptyFrenchCategoriesShouldReturnCreated()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchCategories = new HashSet<string>() };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(0, organization.FrenchCategories.Count);
        }
    }
}
