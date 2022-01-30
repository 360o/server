namespace _360o.Server.API.V1.Stores.Services
{
    public record struct ListStoresInput
    {
        public string? Query { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Radius { get; set; }
    }
}
