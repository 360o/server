using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.DTOs
{
    public record class ItemDTO(
        Guid Id,
        string? EnglishName,
        string? EnglishDescription,
        string? FrenchName,
        string? FrenchDescription,
        MoneyValueDTO? Price
        );
}