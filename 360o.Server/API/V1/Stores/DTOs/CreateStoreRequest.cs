using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct CreateStoreRequest(
        Guid OrganizationId,
        CreateStoreRequestPlace Place
        )
    {
    }

    public record struct CreateStoreRequestPlace(
        string? GooglePlaceId,
        string? FormattedAddress,
        Location Location
        )
    {
    }
}

