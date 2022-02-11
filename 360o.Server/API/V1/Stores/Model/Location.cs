using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public record class Location
    {
        public Location(double latitude, double longitude)
        {
            // See https://docs.mapbox.com/help/glossary/lat-lon/#:~:text=Latitude%20and%20longitude%20are%20a,180%20to%20180%20for%20longitude
            Latitude = Guard.Against.OutOfRange(latitude, nameof(latitude), -90, 90);
            Longitude = Guard.Against.OutOfRange(longitude, nameof(longitude), -180, 180); ;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}