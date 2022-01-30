namespace _360o.Server.API.V1.Organizations.DTOs
{
    public record struct CreateOrganizationRequest
    {
        public string? Name { get; set; }

        public string? EnglishShortDescription { get; set; }
        public string? EnglishLongDescription { get; set; }
        public ISet<string>? EnglishCategories { get; set; }

        public string? FrenchShortDescription { get; set; }
        public string? FrenchLongDescription { get; set; }
        public ISet<string>? FrenchCategories { get; set; }
    }
}
