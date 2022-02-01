using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateStoreInput(
        Guid OrganizationId,
        CreateStoreInputPlace Place
        )
    {
    }

    public readonly record struct CreateStoreInputPlace(
        string GooglePlaceId,
        string FormattedAddress,
        Location Location)
    {
    }
}
