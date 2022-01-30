using NetTopologySuite.Geometries;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Place : BaseEntity
    {
        private Place(string googlePlaceId, string formattedAddress, Point point)
        {
            GooglePlaceId = googlePlaceId;
            FormattedAddress = formattedAddress;
            Point = point;
        }

        public Place(string googlePlaceId, string formattedAddress, Location location)
        {
            GooglePlaceId = googlePlaceId;
            FormattedAddress = formattedAddress;
            // See https://docs.microsoft.com/en-us/ef/core/modeling/spatial#longitude-and-latitude
            Point = new Point(x: location.Longitude, y: location.Latitude);
        }

        public string GooglePlaceId { get; private set; }
        public string FormattedAddress { get; private set; }
        public Point Point { get; private set; }
        public Location Location => new Location(latitude: Point.Y, longitude: Point.X);
        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }
    }
}

