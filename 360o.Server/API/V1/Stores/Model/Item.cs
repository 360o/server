namespace _360o.Server.API.V1.Stores.Model
{
    public class Item : BaseEntity
    {
        private Item()
        {
        }

        public Item(Guid storeId, string name, string? englishDescription, string? frenchDescription, MoneyValue? price)
        {
            if (price.HasValue)
            {
                if (price.Value.Amount < 0)
                {
                    throw new ArgumentException($"Amount must be greater or equal to 0", nameof(price));
                }
            }

            StoreId = storeId;
            Name = name;
            EnglishDescription = englishDescription;
            FrenchDescription = frenchDescription;
            Price = price;
        }

        public string Name { get; private set; }
        public string? EnglishDescription { get; private set; }
        public string? FrenchDescription { get; private set; }
        public MoneyValue? Price { get; private set; }

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }

        public List<OfferItem> OfferItems { get; private set; }
    }
}
