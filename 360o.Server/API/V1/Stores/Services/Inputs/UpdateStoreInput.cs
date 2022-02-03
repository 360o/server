using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services.Inputs
{
    public readonly record struct UpdateStoreInput(
        Guid StoreId,
        Place? Place
        )
    {
    }
}
