namespace _360o.Server.Api.V1.Stores.Model
{
    public readonly record struct MoneyValue(
        decimal Amount,
        Iso4217CurrencyCode CurrencyCode
        );
}