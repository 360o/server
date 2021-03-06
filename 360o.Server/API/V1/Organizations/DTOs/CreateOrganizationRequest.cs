namespace _360o.Server.Api.V1.Organizations.DTOs
{
    public readonly record struct CreateOrganizationRequest(
        string Name,
        string? EnglishShortDescription,
        string? EnglishLongDescription,
        ISet<string>? EnglishCategories,
        string? FrenchShortDescription,
        string? FrenchLongDescription,
        ISet<string>? FrenchCategories
        );
}