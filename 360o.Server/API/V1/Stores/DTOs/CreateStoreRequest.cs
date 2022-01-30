using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public record struct CreateStoreRequest
    {
        public Guid OrganizationId { get; set; }
        public CreateStoreRequestPlace Place { get; set; }
    }

    public record struct CreateStoreRequestPlace
    {
        public string? GooglePlaceId { get; set; }
        public string? FormattedAddress { get; set; }
        public Location Location { get; set; }
    }
}

