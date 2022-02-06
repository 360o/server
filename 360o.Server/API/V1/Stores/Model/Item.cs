using Ardalis.GuardClauses;
using FluentValidation;
using NpgsqlTypes;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Item : BaseEntity
    {
        public Item(Guid storeId)
        {
            Guard.Against.NullOrEmpty(storeId, nameof(storeId));

            StoreId = storeId;
        }

        private Item()
        {
        }

        public string EnglishName { get; private set; } = string.Empty;
        public string EnglishDescription { get; private set; } = string.Empty;
        public NpgsqlTsVector EnglishSearchVector { get; private set; }
        public string FrenchName { get; private set; } = string.Empty;
        public string FrenchDescription { get; private set; } = string.Empty;
        public NpgsqlTsVector FrenchSearchVector { get; private set; }
        public MoneyValue? Price { get; private set; }

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }

        public List<OfferItem> OfferItems { get; private set; }

        public void SetEnglishName(string englishName)
        {
            EnglishName = Guard.Against.Null(englishName, nameof(englishName)).Trim();
        }

        public void SetEnglishDescription(string englishDescription)
        {
            EnglishDescription = Guard.Against.Null(englishDescription, nameof(englishDescription)).Trim();
        }

        public void SetFrenchName(string frenchName)
        {
            FrenchName = Guard.Against.Null(frenchName, nameof(frenchName)).Trim();
        }

        public void SetFrenchDescription(string frenchDescription)
        {
            FrenchDescription = Guard.Against.Null(frenchDescription, nameof(frenchDescription)).Trim();
        }

        public void SetPrice(MoneyValue price)
        {
            Guard.Against.NegativeOrZero(price.Amount, nameof(price.Amount));
            Guard.Against.EnumOutOfRange(price.CurrencyCode, nameof(price.CurrencyCode));

            Price = price;
        }
    }
}