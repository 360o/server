namespace _360o.Server.API.V1.Stores.DTOs
{
    public record struct StoreDTO
    {
        public Guid Id { get; set; }

        public PlaceDTO Place { get; set; }
    }
}

