using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct CreateOrUpdateOfferInput(
        string? EnglishName,
        string? FrenchName,
        ISet<CreateOrUpdateOfferInputItem> OfferItems,
        MoneyValue? Discount
        )
    {
    }

    public readonly record struct CreateOrUpdateOfferInputItem(
        Guid ItemId,
        int Quantity
        );
}