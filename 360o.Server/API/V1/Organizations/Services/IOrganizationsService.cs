using _360o.Server.Api.V1.Organizations.Model;
using _360o.Server.Api.V1.Organizations.Services.Inputs;

namespace _360o.Server.Api.V1.Organizations.Services
{
    public interface IOrganizationsService
    {
        public Task<Organization> CreateOrganizationAsync(CreateOrganizationInput input);

        public Task<Organization?> GetOrganizationByIdAsync(Guid organizationId);

        public Task<Organization> UpdateOrganizationAsync(Organization organization);

        public Task DeleteOrganizationByIdAsync(Guid organizationId);
    }
}