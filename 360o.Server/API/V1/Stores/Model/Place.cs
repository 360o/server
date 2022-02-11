using Ardalis.GuardClauses;
using NetTopologySuite.Geometries;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Place : BaseEntity
    {
        public Place(string googlePlaceId, string formattedAddress, Location location) : this(googlePlaceId, formattedAddress, ToPoint(location))
        {
        }

        private Place(string googlePlaceId, string formattedAddress, Point point)
        {
            GooglePlaceId = Guard.Against.NullOrWhiteSpace(googlePlaceId, nameof(googlePlaceId));
            FormattedAddress = Guard.Against.NullOrWhiteSpace(formattedAddress, nameof(formattedAddress));
            Point = point;
        }

        public string GooglePlaceId { get; private set; }

        public string FormattedAddress { get; private set; }

        public Point Point { get; private set; }

        public Location Location => ToLocation(Point);

        public Guid StoreId { get; private set; }

        public Store Store { get; private set; }

        // See https://docs.microsoft.com/en-us/ef/core/modeling/spatial#longitude-and-latitude
        private static Location ToLocation(Point point) => new Location(point.Y, point.X);

        private static Point ToPoint(Location location) => new Point(x: location.Longitude, y: location.Latitude);
    }
}