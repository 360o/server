using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Controllers.DTOs
{
    public record struct CreateStoreRequest
    {
        public string DisplayName { get; set; }

        public string? EnglishShortDescription { get; set; }

        public string? EnglishLongDescription { get; set; }

        public ISet<string>? EnglishCategories { get; set; }

        public string? FrenchShortDescription { get; set; }

        public string? FrenchLongDescription { get; set; }

        public ISet<string>? FrenchCategories { get; set; }

        public CreateMerchantPlace Place { get; set; }
    }

    public record struct CreateMerchantPlace
    {
        public string GooglePlaceId { get; set; }

        public string FormattedAddress { get; set; }

        public LocationDTO Location { get; set; }
    }
}

