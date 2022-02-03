using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct PlaceDTO(
        string GooglePlaceId,
        string FormattedAddress,
        Location Location
        )
    {
    }
}