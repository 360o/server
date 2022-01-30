namespace _360o.Server.API.V1.Stores.DTOs
{
    public record struct CreateStoreRequest
    {
        public Guid OrganizationId { get; set; }

        public CreateStorePlace Place { get; set; }
    }

    public record struct CreateStorePlace
    {
        public string? GooglePlaceId { get; set; }

        public string? FormattedAddress { get; set; }

        public LocationDTO Location { get; set; }
    }
}

