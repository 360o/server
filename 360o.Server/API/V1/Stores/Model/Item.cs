using NpgsqlTypes;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Item : BaseEntity
    {
        public Item(Guid storeId, string? englishName, string? englishDescription, string? frenchName, string? frenchDescription, MoneyValue? price)
        {
            if (englishName == null && frenchName == null)
            {
                throw new ArgumentNullException("At least one of Name must be defined");
            }

            if (price.HasValue)
            {
                if (price.Value.Amount < 0)
                {
                    throw new ArgumentException($"Amount must be greater or equal to 0", nameof(price));
                }
            }

            StoreId = storeId;
            EnglishName = englishName == null ? string.Empty : englishName.Trim();
            EnglishDescription = englishDescription == null ? string.Empty : englishDescription.Trim();
            FrenchDescription = frenchDescription == null ? string.Empty : frenchDescription.Trim();
            FrenchName = frenchName == null ? string.Empty : frenchName.Trim();
            Price = price;
        }

        private Item()
        {
        }

        public string EnglishName { get; private set; }
        public string EnglishDescription { get; private set; }
        public NpgsqlTsVector EnglishSearchVector { get; private set; }
        public string FrenchName { get; private set; }
        public string FrenchDescription { get; private set; }
        public NpgsqlTsVector FrenchSearchVector { get; private set; }
        public MoneyValue? Price { get; private set; }

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }

        public List<OfferItem> OfferItems { get; private set; }
    }
}