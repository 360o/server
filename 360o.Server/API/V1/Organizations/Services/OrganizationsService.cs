using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Organizations.Services.Inputs;
using _360o.Server.API.V1.Stores.Model;
using Microsoft.EntityFrameworkCore;

namespace _360o.Server.API.V1.Organizations.Services
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
            var organization = new Organization(
                input.UserId,
                input.Name,
                input.EnglishShortDescription,
                input.EnglishLongDescription,
                input.FrenchShortDescription,
                input.FrenchLongDescription
                );

            if (input.EnglishCategories != null)
            {
                organization.SetEnglishCategories(input.EnglishCategories);
            }

            if (input.FrenchCategories != null)
            {
                organization.SetFrenchCategories(input.FrenchCategories);
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

        public async Task<Organization> UpdateOrganizationAsync(UpdateOrganizationInput input)
        {
            var organization = await _apiContext.Organizations.FindAsync(input.OrganizationId);

            if (organization == null)
            {
                throw new KeyNotFoundException("Organization not found");
            }

            if (input.Name != null)
            {
                organization.SetName(input.Name);
            }

            if (input.EnglishShortDescription != null)
            {
                organization.SetEnglishShortDescription(input.EnglishShortDescription);
            }

            if (input.EnglishLongDescription != null)
            {
                organization.SetEnglishLongDescription(input.EnglishLongDescription);
            }

            if (input.EnglishCategories != null)
            {
                organization.SetEnglishCategories(input.EnglishCategories);
            }

            if (input.FrenchShortDescription != null)
            {
                organization.SetFrenchShortDescription(input.FrenchShortDescription);
            }

            if (input.FrenchLongDescription != null)
            {
                organization.SetFrenchLongDescription(input.FrenchLongDescription);
            }

            if (input.FrenchCategories != null)
            {
                organization.SetFrenchCategories(input.FrenchCategories);
            }

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
