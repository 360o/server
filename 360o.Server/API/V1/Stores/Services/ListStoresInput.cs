namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct ListStoresInput(
        string? Query,
        double? Latitude,
        double? Longitude,
        double? Radius)
    {
    }
}
