using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Offer : BaseEntity
    {
        private List<OfferItem> _offerItems = new List<OfferItem>();

        public Offer(Guid storeId)
        {
            StoreId = storeId;
        }

        private Offer()
        {
        }

        public string EnglishName { get; private set; } = string.Empty;

        public string FrenchName { get; private set; } = string.Empty;

        public MoneyValue? Discount { get; private set; }

        public Guid StoreId { get; private set; }

        public Store Store { get; private set; }

        public IReadOnlyList<OfferItem> OfferItems => _offerItems.AsReadOnly();

        public void SetEnglishName(string englishName)
        {
            EnglishName = Guard.Against.Null(englishName, nameof(englishName));
        }

        public void SetFrenchName(string frenchName)
        {
            FrenchName = Guard.Against.Null(frenchName, nameof(frenchName));
        }

        public void SetOfferItems(ISet<OfferItem> offerItems)
        {
            _offerItems = offerItems.ToList();
        }

        public void SetDiscount(MoneyValue? discount)
        {
            if (discount.HasValue)
            {
                Guard.Against.NegativeOrZero(discount.Value.Amount, nameof(discount.Value.Amount));
                Guard.Against.EnumOutOfRange(discount.Value.CurrencyCode, nameof(discount.Value.CurrencyCode));
            }

            Discount = discount;
        }
    }
}