namespace _360o.Server.Api.V1.Organizations.DTOs
{
    public readonly record struct OrganizationDTO(
        Guid Id,
        string Name,
        string EnglishShortDescription,
        string EnglishLongDescription,
        IList<string> EnglishCategories,
        string FrenchShortDescription,
        string FrenchLongDescription,
        IList<string> FrenchCategories
        )
    {
    }
}