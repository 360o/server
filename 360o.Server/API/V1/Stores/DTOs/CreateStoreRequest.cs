namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct CreateStoreRequest(
        Guid OrganizationId,
        PlaceDTO Place
        );
}