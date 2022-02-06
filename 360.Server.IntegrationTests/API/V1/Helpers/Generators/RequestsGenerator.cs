using _360o.Server.Api.V1.Organizations.DTOs;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _360.Server.IntegrationTests.Api.V1.Helpers.Generators
{
    internal static class RequestsGenerator
    {
        private static Faker EnglishFaker => new Faker();
        private static Faker FrenchFaker => new Faker("fr");

        public static CreateOrganizationRequest MakeRandomCreateOrganizationRequest()
        {
            return new CreateOrganizationRequest
            {
                Name = EnglishFaker.Company.CompanyName(),
                EnglishShortDescription = EnglishFaker.Company.CatchPhrase(),
                EnglishLongDescription = EnglishFaker.Commerce.ProductDescription(),
                EnglishCategories = EnglishFaker.Commerce.Categories(EnglishFaker.Random.Int(0, 5)).ToHashSet(),
                FrenchShortDescription = FrenchFaker.Company.CatchPhrase(),
                FrenchLongDescription = FrenchFaker.Commerce.ProductDescription(),
                FrenchCategories = FrenchFaker.Commerce.Categories(FrenchFaker.Random.Int(0, 5)).ToHashSet(),
            };
        }

        public static CreateStoreRequest MakeRandomCreateStoreRequest(Guid organizationId)
        {
            return new CreateStoreRequest
            {
                OrganizationId = organizationId,
                Place = new PlaceDTO
                {
                    GooglePlaceId = EnglishFaker.Random.Uuid().ToString(),
                    FormattedAddress = EnglishFaker.Address.FullAddress(),
                    Location = new Location(EnglishFaker.Address.Latitude(), EnglishFaker.Address.Longitude())
                },
            };
        }

        public static CreateItemRequest MakeRandomCreateItemRequest()
        {
            return new CreateItemRequest
            {
                EnglishName = EnglishFaker.Commerce.ProductName(),
                EnglishDescription = EnglishFaker.Commerce.ProductDescription(),
                FrenchName = FrenchFaker.Commerce.ProductName(),
                FrenchDescription = FrenchFaker.Commerce.ProductDescription(),
                Price = new MoneyValue
                {
                    Amount = EnglishFaker.Random.Decimal(0, 100),
                    CurrencyCode = EnglishFaker.PickRandom<Iso4217CurrencyCode>()
                }
            };
        }

        public static CreateOfferRequest MakeRandomCreateOfferRequest(ISet<CreateOfferRequestItem> offerItems, MoneyValue? discount)
        {
            return new CreateOfferRequest
            {
                EnglishName = EnglishFaker.Commerce.ProductAdjective(),
                FrenchName = FrenchFaker.Commerce.ProductAdjective(),
                OfferItems = offerItems,
                Discount = discount
            };
        }
    }
}