namespace _360o.Server.API.V1.Organizations.Services
{
    public readonly record struct CreateOrganizationInput(
        string UserId,
        string Name,
        string? EnglishShortDescription,
        string? EnglishLongDescription,
        ISet<string>? EnglishCategories,
        string? FrenchShortDescription,
        string? FrenchLongDescription,
        ISet<string>? FrenchCategories
        )
    {
    }
}
