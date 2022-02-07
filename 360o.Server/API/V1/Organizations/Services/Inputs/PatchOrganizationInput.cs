namespace _360o.Server.Api.V1.Organizations.Services.Inputs
{
    public readonly record struct PatchOrganizationInput(
        Guid OrganizationId,
        string? Name,
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