namespace _360o.Server.Api.V1.Stores.DTOs
{
    public record class ListStoresRequest
    {
        public string? Query { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Radius { get; set; }
    }
}