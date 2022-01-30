using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using Bogus;
using System;
using System.Linq;

namespace _360.Server.IntegrationTests.API.V1.Helpers.Generators
{
    internal static class RequestsGenerator
    {
        public static CreateOrganizationRequest MakeRandomCreateOrganizationRequest()
        {
            var englishFaker = new Faker();
            var frenchFaker = new Faker(locale: "fr");

            var request = new CreateOrganizationRequest
            {
                Name = englishFaker.Company.CompanyName(),
                EnglishShortDescription = englishFaker.Company.CatchPhrase(),
                EnglishLongDescription = englishFaker.Commerce.ProductDescription(),
                EnglishCategories = englishFaker.Commerce.Categories(englishFaker.Random.Int(0, 5)).ToHashSet(),
                FrenchShortDescription = frenchFaker.Company.CatchPhrase(),
                FrenchLongDescription = frenchFaker.Commerce.ProductDescription(),
                FrenchCategories = frenchFaker.Commerce.Categories(frenchFaker.Random.Int(0, 5)).ToHashSet(),
            };

            return request;
        }

        public static CreateStoreRequest MakeRandomCreateStoreRequest(Guid organizationId)
        {
            var faker = new Faker();

            var request = new CreateStoreRequest
            {
                OrganizationId = organizationId,
                Place = new CreateStoreRequestPlace
                {
                    GooglePlaceId = faker.Random.Uuid().ToString(),
                    FormattedAddress = faker.Address.FullAddress(),
                    Location = new Location(faker.Address.Latitude(), faker.Address.Longitude())
                },
            };

            return request;
        }
    }
}
