using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct CreateOfferInput(
        string? EnglishName,
        string? FrenchName,
        ISet<CreateOfferItem> OfferItems,
        MoneyValue? Discount
        );

    public readonly record struct CreateOfferItem(
        Guid ItemId,
        int Quantity
        );
}