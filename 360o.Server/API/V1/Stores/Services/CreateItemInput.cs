using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateItemInput(
        Guid StoreId,
        string? EnglishName,
        string? EnglishDescription,
        string? FrenchName,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}
