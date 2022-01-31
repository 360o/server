using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateItemInput
    {
        public CreateItemInput(Guid storeId, string name, string? englishDescription, string? frenchDescription, MoneyValue? price)
        {
            StoreId = storeId;
            Name = name;
            EnglishDescription = englishDescription;
            FrenchDescription = frenchDescription;
            Price = price;
        }

        public Guid StoreId { get; }
        public string Name { get; }
        public string? EnglishDescription { get; }
        public string? FrenchDescription { get; }
        public MoneyValue? Price { get; }
    }
}
