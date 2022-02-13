namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct CreateOfferRequest(
        string? EnglishName,
        string? FrenchName,
        IEnumerable<CreateOfferRequestItem> OfferItems,
        MoneyValueDTO? Discount
        );

    public readonly record struct CreateOfferRequestItem(
        Guid ItemId,
        int Quantity
        );
}