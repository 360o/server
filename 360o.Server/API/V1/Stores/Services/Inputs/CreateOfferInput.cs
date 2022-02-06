using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct CreateOfferInput(
        Guid StoreId,
        string? EnglishName,
        string? FrenchName,
        IReadOnlyCollection<CreateOfferInputItems> Items,
        MoneyValue? Discount
        )
    {
    }

    public readonly record struct CreateOfferInputItems(
        Guid itemId,
        int Quantity
        );
}