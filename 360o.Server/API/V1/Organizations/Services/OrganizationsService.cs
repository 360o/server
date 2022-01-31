﻿using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;

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
                foreach (var category in input.EnglishCategories)
                {
                    organization.AddEnglishCategory(category);
                }
            }

            if (input.FrenchCategories != null)
            {
                foreach (var category in input.FrenchCategories)
                {
                    organization.AddFrenchCategory(category);
                }
            }

            _apiContext.Add(organization);

            await _apiContext.SaveChangesAsync();

            return organization;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(Guid id)
        {
            return await _apiContext.Organizations.FindAsync(id);
        }
    }
}