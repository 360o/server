using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;

namespace _360o.Server.Merchants.API.V1.Controllers
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

        public ISet<PlaceDTO> Places { get; set; }
    }
}

