using Ardalis.GuardClauses;
using NpgsqlTypes;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Item : BaseEntity
    {
        private string? _englishName;
        private string? _englishDescription;
        private string? _frenchName;
        private string? _frenchDescription;
        private MoneyValue? _price;

        public Item(Guid storeId)
        {
            Guard.Against.NullOrEmpty(storeId, nameof(storeId));

            StoreId = storeId;
        }

        private Item()
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

        public string? EnglishDescription
        {
            get => _englishDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _englishDescription = null;
                }
                else
                {
                    _englishDescription = value.Trim();
                }
            }
        }

        public NpgsqlTsVector EnglishSearchVector { get; private set; }

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

        public string? FrenchDescription
        {
            get => _frenchDescription;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _frenchDescription = null;
                }
                else
                {
                    _frenchDescription = value.Trim();
                }
            }
        }

        public NpgsqlTsVector FrenchSearchVector { get; private set; }

        public MoneyValue? Price
        {
            get => _price;
            set
            {
                if (value.HasValue)
                {
                    Guard.Against.NegativeOrZero(value.Value.Amount, nameof(value.Value.Amount));
                    Guard.Against.EnumOutOfRange(value.Value.CurrencyCode, nameof(value.Value.CurrencyCode));

                    _price = value;
                }
                else
                {
                    _price = null;
                }
            }
        }

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }

        public List<OfferItem> OfferItems { get; private set; }
    }
}