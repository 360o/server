using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateItemInput(
        Guid StoreId,
        string Name,
        string? EnglishDescription,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}
