using _360o.Server.API.V1.Stores.Validators;
using FluentValidation;
using NpgsqlTypes;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Item : BaseEntity
    {
        private MoneyValueValidator _moneyValueValidator = new MoneyValueValidator();

        public Item(Guid storeId, string? englishName, string? englishDescription, string? frenchName, string? frenchDescription, MoneyValue? price)
        {
            if (englishName == null && frenchName == null)
            {
                throw new ArgumentNullException("At least one of Name must be defined");
            }

            if (price.HasValue)
            {
                AssertValidPrice(price.Value);
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

        public void SetEnglishName(string englishName)
        {
            EnglishName = englishName;
        }

        public void SetEnglishDescription(string englishDescription)
        {
            EnglishDescription = englishDescription;
        }

        public void SetFrenchName(string frenchName)
        {
            FrenchName = frenchName;
        }

        public void SetFrenchDescription(string frenchDescription)
        {
            FrenchDescription = frenchDescription;
        }

        public void SetPrice(MoneyValue price)
        {
            AssertValidPrice(price);

            Price = price;
        }

        private void AssertValidPrice(MoneyValue price)
        {
            _moneyValueValidator.ValidateAndThrow(price);

            Price = price;
        }
    }
}