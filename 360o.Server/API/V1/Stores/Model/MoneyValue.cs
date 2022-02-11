using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public record class MoneyValue
    {
        public MoneyValue(decimal amount, Iso4217CurrencyCode currencyCode)
        {
            Amount = Guard.Against.Negative(amount, nameof(amount));
            CurrencyCode = Guard.Against.EnumOutOfRange(currencyCode, nameof(currencyCode));
        }

        public decimal Amount { get; }

        public Iso4217CurrencyCode CurrencyCode { get; }
    }
}