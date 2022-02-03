namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct CreateStoreRequest(
        Guid OrganizationId,
        PlaceDTO Place
        )
    {
    }
}