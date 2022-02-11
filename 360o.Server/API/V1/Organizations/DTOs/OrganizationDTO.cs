namespace _360o.Server.Api.V1.Organizations.DTOs
{
    public readonly record struct OrganizationDTO(
        Guid Id,
        string Name,
        string? EnglishShortDescription,
        string? EnglishLongDescription,
        IEnumerable<string>? EnglishCategories,
        string? FrenchShortDescription,
        string? FrenchLongDescription,
        IEnumerable<string>? FrenchCategories
        );
}