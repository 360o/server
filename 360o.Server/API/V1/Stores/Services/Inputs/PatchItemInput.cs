using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct PatchItemInput(
        string? EnglishName,
        string? EnglishDescription,
        string? FrenchName,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}