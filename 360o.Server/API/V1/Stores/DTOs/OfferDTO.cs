using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.DTOs
{
    public record class OfferDTO(
        Guid Id,
        string? EnglishName,
        string? FrenchName,
        IEnumerable<OfferItemDTO> OfferItems,
        MoneyValueDTO? Discount,
        Guid StoreId
        );
}