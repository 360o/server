using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct CreateItemRequest(
        string Name,
        string? EnglishDescription,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}
