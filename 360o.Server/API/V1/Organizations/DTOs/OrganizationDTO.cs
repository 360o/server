namespace _360o.Server.API.V1.Organizations.DTOs
{
    public record struct OrganizationDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string EnglishShortDescription { get; set; }
        public string EnglishLongDescription { get; set; }
        public IList<string> EnglishCategories { get; set; }

        public string FrenchShortDescription { get; set; }
        public string FrenchLongDescription { get; set; }
        public IList<string> FrenchCategories { get; set; }
    }
}
