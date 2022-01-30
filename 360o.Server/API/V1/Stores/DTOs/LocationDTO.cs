namespace _360o.Server.API.V1.Stores.DTOs
{
    public record struct LocationDTO
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
