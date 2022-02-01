using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct StoreDTO(
        Guid Id,
        PlaceDTO Place,
        Guid OrganizationId
        )
    {
    }

    public readonly record struct PlaceDTO(
        Guid Id,
        string GooglePlaceId,
        string FormattedAddress,
        Location Location
        )
    {
    }
}

