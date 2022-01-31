namespace _360o.Server.API.V1.Stores.Model
{
    public record struct MoneyValue
    {
        public decimal Amount { get; set; }
        public Iso4217CurrencyCode CurrencyCode { get; set; }
    }
}
