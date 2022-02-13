namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct OfferItemDTO(
        Guid Id,
        int Quantity,
        Guid ItemId);
}