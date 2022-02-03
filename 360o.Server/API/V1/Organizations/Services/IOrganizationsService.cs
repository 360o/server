using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Organizations.Services.Inputs;

namespace _360o.Server.API.V1.Organizations.Services
{
    public interface IOrganizationsService
    {
        public Task<Organization> CreateOrganizationAsync(CreateOrganizationInput input);
        public Task<Organization?> GetOrganizationByIdAsync(Guid id);
        public Task<Organization> UpdateOrganizationAsync(UpdateOrganizationInput input);
        public Task DeleteOrganizationByIdAsync(Guid id);
    }
}
