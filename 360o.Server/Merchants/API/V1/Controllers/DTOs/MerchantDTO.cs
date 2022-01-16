using System;
namespace _360o.Server.Merchants.API.V1.Controllers.DTOs
{
    public record struct MerchantDTO
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string EnglishShortDescription { get; set; }

        public string EnglishLongDescription { get; set; }

        public ISet<string> EnglishCategories { get; set; }

        public string FrenchShortDescription { get; set; }

        public string FrenchLongDescription { get; set; }

        public ISet<string> FrenchCategories { get; set; }

        public ISet<PlaceDTO> Places { get; set; }
    }
}

