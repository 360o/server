using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using _360o.Server.API.V1.Organizations.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    public class OrganizationsHelper
    {
        public static string OrganizationsRoute => "/api/v1/Organizations";

        public static void AssertOrganizationsAreEqual(OrganizationDTO expected, OrganizationDTO actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.EnglishShortDescription, actual.EnglishShortDescription);
            Assert.AreEqual(expected.EnglishLongDescription, actual.EnglishLongDescription);
            CollectionAssert.AreEquivalent(expected.EnglishCategories.ToList(), actual.EnglishCategories.ToList());
            Assert.AreEqual(expected.FrenchShortDescription, actual.FrenchShortDescription);
            Assert.AreEqual(expected.FrenchLongDescription, actual.FrenchLongDescription);
            CollectionAssert.AreEquivalent(expected.FrenchCategories.ToList(), actual.FrenchCategories.ToList());
        }

        private readonly IAuthHelper _authHelper;

        public OrganizationsHelper(IAuthHelper authService)
        {
            _authHelper = authService;
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
            Assert.AreEqual($"{OrganizationsRoute}/{organization.Id}", response.Headers.Location.AbsolutePath);

            return organization;
        }

        public async Task<OrganizationDTO> CreateRandomOrganizationAndDeserializeAsync()
        {
            var request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            return await CreateOrganizationAndDeserializeAsync(request);
        }

        public async Task<HttpResponseMessage> GetOrganizationByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient().GetAsync($"{OrganizationsRoute}/{id}");
        }

        public async Task<OrganizationDTO> GetOrganizationByIdAndDeserializeAsync(Guid id)
        {
            var response = await GetOrganizationByIdAsync(id);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await Utils.DeserializeAsync<OrganizationDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteOrganizationByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync($"{OrganizationsRoute}/{id}");
        }
    }
}
