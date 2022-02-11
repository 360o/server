namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct PlaceDTO(
        string GooglePlaceId,
        string FormattedAddress,
        LocationDTO Location
        );
}