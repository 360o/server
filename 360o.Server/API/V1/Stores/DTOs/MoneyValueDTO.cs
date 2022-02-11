using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct MoneyValueDTO(
        decimal Amount,
        Iso4217CurrencyCode CurrencyCode);
}