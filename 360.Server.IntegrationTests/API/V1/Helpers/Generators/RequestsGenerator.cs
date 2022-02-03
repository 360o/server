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
            var frenchFaker = new Faker("fr");

            return new CreateOrganizationRequest
            {
                Name = englishFaker.Company.CompanyName(),
                EnglishShortDescription = englishFaker.Company.CatchPhrase(),
                EnglishLongDescription = englishFaker.Commerce.ProductDescription(),
                EnglishCategories = englishFaker.Commerce.Categories(englishFaker.Random.Int(0, 5)).ToHashSet(),
                FrenchShortDescription = frenchFaker.Company.CatchPhrase(),
                FrenchLongDescription = frenchFaker.Commerce.ProductDescription(),
                FrenchCategories = frenchFaker.Commerce.Categories(frenchFaker.Random.Int(0, 5)).ToHashSet(),
            };
        }

        public static CreateStoreRequest MakeRandomCreateStoreRequest(Guid organizationId)
        {
            var faker = new Faker();

            return new CreateStoreRequest
            {
                OrganizationId = organizationId,
                Place = new PlaceDTO
                {
                    GooglePlaceId = faker.Random.Uuid().ToString(),
                    FormattedAddress = faker.Address.FullAddress(),
                    Location = new Location(faker.Address.Latitude(), faker.Address.Longitude())
                },
            };
        }

        public static CreateItemRequest MakeRandomCreateItemRequest()
        {
            var englishFaker = new Faker();
            var frenchFaker = new Faker("fr");

            return new CreateItemRequest
            {
                EnglishName = englishFaker.Commerce.ProductName(),
                EnglishDescription = englishFaker.Commerce.ProductDescription(),
                FrenchName = frenchFaker.Commerce.ProductName(),
                FrenchDescription = frenchFaker.Commerce.ProductDescription(),
                Price = new MoneyValue
                {
                    Amount = englishFaker.Random.Decimal(0, 100),
                    CurrencyCode = englishFaker.PickRandom<Iso4217CurrencyCode>()
                }
            };
        }
    }
}