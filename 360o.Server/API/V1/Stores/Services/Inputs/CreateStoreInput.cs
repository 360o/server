using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct CreateStoreInput(
        Guid OrganizationId,
        Place Place
        );
}