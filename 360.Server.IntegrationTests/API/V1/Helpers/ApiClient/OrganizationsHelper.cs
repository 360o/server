using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Organizations.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
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

        public async Task<HttpResponseMessage> CreateOrganizationAsync(CreateOrganizationRequest request)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(request);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PostAsync(OrganizationsRoute, requestContent);
        }

        public async Task<OrganizationDTO> CreateOrganizationAndDeserializeAsync(CreateOrganizationRequest request)
        {
            var response = await CreateOrganizationAsync(request);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(response.Headers.Location);

            var organization = await JsonUtils.DeserializeAsync<OrganizationDTO>(response);

            Assert.IsTrue(Guid.TryParse(organization.Id.ToString(), out var _));
            Assert.AreEqual(OrganizationRoute(organization.Id), response.Headers.Location.AbsolutePath);

            return organization;
        }

        public async Task<OrganizationDTO> CreateRandomOrganizationAndDeserializeAsync()
        {
            var request = Generator.MakeRandomCreateOrganizationRequest();

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

            return await JsonUtils.DeserializeAsync<OrganizationDTO>(response);
        }

        public async Task<HttpResponseMessage> PatchOrganizationAsync(Guid organizationId, object patchDoc)
        {
            var requestContent = JsonUtils.MakeJsonStringContent(patchDoc);

            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).PatchAsync(OrganizationRoute(organizationId), requestContent);
        }

        public async Task<OrganizationDTO> PatchOrganizationAndDeserializeAsync(Guid organizationId, object patchDoc)
        {
            var response = await PatchOrganizationAsync(organizationId, patchDoc);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            return await JsonUtils.DeserializeAsync<OrganizationDTO>(response);
        }

        public async Task<HttpResponseMessage> DeleteOrganizationByIdAsync(Guid id)
        {
            return await ProgramTest.NewClient(await _authHelper.GetAccessToken()).DeleteAsync(OrganizationRoute(id));
        }
    }
}