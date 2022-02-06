using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct CreateOfferRequest(
        string? EnglishName,
        string? FrenchName,
        IReadOnlySet<string> Items,
        MoneyValue? Discount
        )
    {
    }
}