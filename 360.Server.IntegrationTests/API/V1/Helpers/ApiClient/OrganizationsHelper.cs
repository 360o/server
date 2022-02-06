using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Organizations.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient
{
    public class OrganizationsHelper
    {
        private readonly IAuthHelper _authHelper;

        public OrganizationsHelper(IAuthHelper authService)
        {
            _authHelper = authService;
        }

        public static string OrganizationsRoute => "/api/v1/Organizations";

        public static string OrganizationRoute(Guid id) => $"{OrganizationsRoute}/{id}";

        public static void AssertOrganizationsAreEqual(OrganizationDTO expected, OrganizationDTO actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.EnglishShortDescription, actual.EnglishShortDescription);
            Assert.AreEqual(expected.EnglishLongDescription, actual.EnglishLongDescription);
            CollectionAssert.AreEquivalent(expected.EnglishCategories.Select(c => c.Trim().ToLower()).ToList(), actual.EnglishCategories.ToList());
            Assert.AreEqual(expected.FrenchShortDescription, actual.FrenchShortDescription);
            Assert.AreEqual(expected.FrenchLongDescription, actual.FrenchLongDescription);
            CollectionAssert.AreEquivalent(expected.FrenchCategories.Select(c => c.Trim().ToLower()).ToList(), actual.FrenchCategories.ToList());
        }

        public async Task<HttpResponseMessage> CreateOrganizationAsync(CreateOrganizationRequest request)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(OrganizationsRoute, requestContent);
        }

        public async Task<OrganizationDTO> CreateOrganizationAndDeserializeAsync(CreateOrganizationRequest request)
        {
            var response = await CreateOrganizationAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var organization = await Utils.DeserializeAsync<OrganizationDTO>(response);

            Assert.IsTrue(Guid.TryParse(organization.Id.ToString(), out var _));
            Assert.AreEqual(OrganizationRoute(organization.Id), response.Headers.Location.AbsolutePath);

            return organization;
        }

        public async Task<OrganizationDTO> CreateRandomOrganizationAndDeserializeAsync()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            return await CreateOrganizationAndDeserializeAsync(request);
        }

        public async Task<HttpResponseMessage> GetOrganizationByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient().GetAsync(OrganizationRoute(id));
        }

        public async Task<OrganizationDTO> GetOrganizationByIdAndDeserializeAsync(Guid id)
        {
            var response = await GetOrganizationByIdAsync(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await Utils.DeserializeAsync<OrganizationDTO>(response);
        }

        public async Task<HttpResponseMessage> UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationRequest request)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PatchAsync(OrganizationRoute(organizationId), requestContent);
        }

        public async Task<OrganizationDTO> UpdateOrganizationAndDeserializeAsync(Guid organizationId, UpdateOrganizationRequest request)
        {
            var response = await UpdateOrganizationAsync(organizationId, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await Utils.DeserializeAsync<OrganizationDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteOrganizationByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(OrganizationRoute(id));
        }
    }
}