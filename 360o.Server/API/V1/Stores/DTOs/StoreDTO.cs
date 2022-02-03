using _360o.Server.API.V1.Organizations.DTOs;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct StoreDTO(
        Guid Id,
        PlaceDTO Place,
        OrganizationDTO Organization
        )
    {
    }
}