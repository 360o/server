using _360o.Server.API.V1.Organizations.Model;

namespace _360o.Server.API.V1.Organizations.Services
{
    public interface IOrganizationsService
    {
        public Task<Organization> CreateOrganizationAsync(CreateOrganizationInput input);
        public Task<Organization?> GetOrganizationByIdAsync(Guid id);
        public Task DeleteOrganizationByIdAsync(Guid id);
    }
}
