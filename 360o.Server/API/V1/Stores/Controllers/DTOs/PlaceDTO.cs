using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Controllers.DTOs
{
    public record struct PlaceDTO
    {
        public Guid Id { get; set; }

        public string GooglePlaceId { get; set; }

        public string FormattedAddress { get; set; }

        public LocationDTO Location { get; set; }
    }
}

