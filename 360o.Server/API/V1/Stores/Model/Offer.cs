using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Offer : BaseEntity
    {
        private List<OfferItem> _offerItems;
        private MoneyValue? _discount;

        public Offer(Guid storeId)
        {
            StoreId = storeId;
        }

        private Offer()
        {
        }

        public string? EnglishName { get; set; }

        public string? FrenchName { get; set; }

        public MoneyValue? Discount
        {
            get => _discount;
            set
            {
                if (!value.HasValue)
                {
                    _discount = null;
                }
                else
                {
                    Guard.Against.NegativeOrZero(value.Value.Amount, nameof(value.Value.Amount));
                    Guard.Against.EnumOutOfRange(value.Value.CurrencyCode, nameof(value.Value.CurrencyCode));

                    _discount = value.Value;
                }
            }
        }

        public Guid StoreId { get; private set; }

        public Store Store { get; private set; }

        public List<OfferItem> OfferItems
        {
            get => _offerItems;
            set
            {
                var offerItems = Guard.Against.NullOrEmpty(value, nameof(value));

                var duplicates = offerItems
                    .GroupBy(i => i.ItemId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                if (duplicates.Any())
                {
                    throw new ArgumentException($"Duplicate items {string.Join(',', duplicates)}", nameof(value));
                }

                _offerItems = offerItems.ToList();
            }
        }
    }
}