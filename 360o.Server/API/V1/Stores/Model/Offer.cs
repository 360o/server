using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Offer : BaseEntity
    {
        private string? _englishName;
        private string? _frenchName;
        private MoneyValue? _discount;

        public Offer(Guid storeId)
        {
            StoreId = Guard.Against.NullOrEmpty(storeId, nameof(storeId));
        }

        private Offer()
        {
        }

        public string? EnglishName
        {
            get => _englishName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _englishName = null;
                }
                else
                {
                    _englishName = value.Trim();
                }
            }
        }

        public string? FrenchName
        {
            get => _frenchName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _frenchName = null;
                }
                else
                {
                    _frenchName = value.Trim();
                }
            }
        }

        public MoneyValue? Discount
        {
            get => _discount;
            set
            {
                if (value == null || value.Amount == 0)
                {
                    _discount = null;
                }
                else
                {
                    _discount = value;
                }
            }
        }

        public Guid StoreId { get; private set; }

        public Store Store { get; private set; }

        public List<OfferItem> OfferItems { get; init; } = new List<OfferItem>();
    }
}