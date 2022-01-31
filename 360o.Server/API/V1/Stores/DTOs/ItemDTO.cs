using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public class ItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? EnglishDescription { get; set; }
        public string? FrenchDescription { get; set; }
        public MoneyValue? Price { get; set; }
    }
}
