namespace _360o.Server.API.V1.Organizations.Services
{
    public readonly record struct CreateOrganizationInput
    {
        public CreateOrganizationInput(string userId,
            string name,
            string? englishShortDescription,
            string? englishLongDescription,
            ISet<string>? englishCategories,
            string? frenchShortDescription,
            string? frenchLongDescription,
            ISet<string>? frenchCategories)
        {
            UserId = userId;
            Name = name;
            EnglishShortDescription = englishShortDescription;
            EnglishLongDescription = englishLongDescription;
            EnglishCategories = englishCategories;
            FrenchShortDescription = frenchShortDescription;
            FrenchLongDescription = frenchLongDescription;
            FrenchCategories = frenchCategories;
        }

        public string UserId { get; }

        public string Name { get; }

        public string? EnglishShortDescription { get; }
        public string? EnglishLongDescription { get; }
        public ISet<string>? EnglishCategories { get; }

        public string? FrenchShortDescription { get; }
        public string? FrenchLongDescription { get; }
        public ISet<string>? FrenchCategories { get; }
    }
}
