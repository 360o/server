using _360o.Server.Api.V1.Organizations.Model;
using _360o.Server.Api.V1.Organizations.Services.Inputs;
using _360o.Server.Api.V1.Stores.Model;
using Microsoft.EntityFrameworkCore;

namespace _360o.Server.Api.V1.Organizations.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly ApiContext _apiContext;

        public OrganizationsService(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        public async Task<Organization> CreateOrganizationAsync(CreateOrganizationInput input)
        {
            var organization = new Organization(input.UserId, input.Name);

            organization.EnglishShortDescription = input.EnglishShortDescription;

            organization.EnglishLongDescription = input.EnglishLongDescription;

            if (input.EnglishCategories != null)
            {
                organization.EnglishCategories = input.EnglishCategories.ToList();
            }

            if (input.FrenchShortDescription != null)
            {
                organization.FrenchShortDescription = input.FrenchShortDescription;
            }

            if (input.FrenchLongDescription != null)
            {
                organization.FrenchLongDescription = input.FrenchLongDescription;
            }

            if (input.FrenchCategories != null)
            {
                organization.FrenchCategories = input.FrenchCategories.ToList();
            }

            _apiContext.Add(organization);

            await _apiContext.SaveChangesAsync();

            return organization;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(Guid organizationId)
        {
            return await _apiContext.Organizations
                .Where(o => !o.DeletedAt.HasValue)
                .SingleOrDefaultAsync(o => o.Id == organizationId);
        }

        public async Task<Organization> UpdateOrganizationAsync(Organization organization)
        {
            organization.SetUpdated();

            _apiContext.Organizations.Update(organization);

            await _apiContext.SaveChangesAsync();

            return organization;
        }

        public async Task DeleteOrganizationByIdAsync(Guid organizationId)
        {
            var organization = await _apiContext.Organizations.FindAsync(organizationId);

            if (organization == null)
            {
                throw new KeyNotFoundException("Organization not found");
            }

            organization.SetDelete();

            await _apiContext.SaveChangesAsync();
        }
    }
}