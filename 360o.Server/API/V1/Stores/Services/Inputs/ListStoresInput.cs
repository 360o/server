namespace _360o.Server.Api.V1.Stores.Services.Inputs
{
    public readonly record struct ListStoresInput(
        string? Query,
        double? Latitude,
        double? Longitude,
        double? Radius)
    {
    }
}