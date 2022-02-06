﻿using _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Errors.Enums;
using Microsoft.AspNetCore.Mvc;
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
            CollectionAssert.AreEquivalent(request.EnglishCategories.ToList(), organization.EnglishCategories.ToList());
            Assert.AreEqual(request.FrenchShortDescription, organization.FrenchShortDescription);
            Assert.AreEqual(request.FrenchLongDescription, organization.FrenchLongDescription);
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullWhitespaceNameShouldReturnBadRequest(string name)
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

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public async Task GivenNullOrWhitespaceEnglishShortDescriptionShouldReturnCreated(string englishShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishShortDescription = englishShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.EnglishShortDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceEnglishLongDescriptionShouldReturnCreated(string englishLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { EnglishLongDescription = englishLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.EnglishLongDescription);
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

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenNullOrWhitespaceFrenchShortDescriptionShouldReturnCreated(string frenchShortDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchShortDescription = frenchShortDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.FrenchShortDescription);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public async Task GivenEmptyhitespaceFrenchLongDescriptionShouldReturnCreated(string frenchLongDescription)
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            request = request with { FrenchLongDescription = frenchLongDescription };

            var organization = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(request);

            Assert.AreEqual(string.Empty, organization.FrenchLongDescription);
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