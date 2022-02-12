using _360o.Server.Api.V1.Organizations.DTOs;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _360.Server.IntegrationTests.Api.V1.Helpers.Generators
{
    internal static class Generator
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
                EnglishCategories = EnglishFaker.Commerce.Categories(EnglishFaker.Random.Int(1, 20)).ToHashSet(),
                FrenchShortDescription = FrenchFaker.Company.CatchPhrase(),
                FrenchLongDescription = FrenchFaker.Commerce.ProductDescription(),
                FrenchCategories = FrenchFaker.Commerce.Categories(FrenchFaker.Random.Int(1, 20)).ToHashSet(),
            };
        }

        public static CreateStoreRequest MakeRandomCreateStoreRequest(Guid organizationId)
        {
            return new CreateStoreRequest(organizationId, MakeRandomPlace());
        }

        public static CreateItemRequest MakeRandomCreateItemRequest()
        {
            return new CreateItemRequest
            {
                EnglishName = EnglishFaker.Commerce.ProductName(),
                EnglishDescription = EnglishFaker.Commerce.ProductDescription(),
                FrenchName = FrenchFaker.Commerce.ProductName(),
                FrenchDescription = FrenchFaker.Commerce.ProductDescription(),
                Price = new MoneyValueDTO
                {
                    Amount = EnglishFaker.Random.Decimal(0, 100),
                    CurrencyCode = EnglishFaker.PickRandom<Iso4217CurrencyCode>()
                }
            };
        }

        public static CreateOfferRequest MakeRandomCreateOfferRequest(ISet<CreateOfferRequestItem> offerItems, MoneyValueDTO? discount)
        {
            return new CreateOfferRequest
            {
                EnglishName = EnglishFaker.Commerce.ProductAdjective(),
                FrenchName = FrenchFaker.Commerce.ProductAdjective(),
                OfferItems = offerItems,
                Discount = discount
            };
        }

        public static PlaceDTO MakeRandomPlace()
        {
            return new PlaceDTO
            {
                GooglePlaceId = EnglishFaker.Random.Uuid().ToString(),
                FormattedAddress = EnglishFaker.Address.FullAddress(),
                Location = MakeRandomLocation()
            };
        }

        public static LocationDTO MakeRandomLocation()
        {
            return new LocationDTO
            {
                Latitude = EnglishFaker.Address.Latitude(),
                Longitude = EnglishFaker.Address.Longitude(),
            };
        }
    }
}