using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public record struct StoreDTO
    {
        public Guid Id { get; set; }
        public PlaceDTO Place { get; set; }
    }

    public record struct PlaceDTO
    {
        public Guid Id { get; set; }
        public string GooglePlaceId { get; set; }
        public string FormattedAddress { get; set; }
        public Location Location { get; set; }
    }
}

