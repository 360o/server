using _360o.Server.Merchants.API.V1.Model;

namespace _360o.Server.API.V1.Merchants.Controllers.DTOs
{
    public record struct CreateMerchantRequest
    {
        public string DisplayName { get; set; }

        public string? EnglishShortDescription { get; set; }

        public string? EnglishLongDescription { get; set; }

        public ISet<string>? EnglishCategories { get; set; }

        public string? FrenchShortDescription { get; set; }

        public string? FrenchLongDescription { get; set; }

        public ISet<string>? FrenchCategories { get; set; }

        public ISet<CreateMerchantPlace> Places { get; set; }
    }

    public record struct CreateMerchantPlace
    {
        public string GooglePlaceId { get; set; }

        public string FormattedAddress { get; set; }

        public Location Location { get; set; }
    }
}

