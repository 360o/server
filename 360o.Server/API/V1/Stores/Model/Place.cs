using NetTopologySuite.Geometries;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Place : BaseEntity
    {
        public Place(string googlePlaceId, string formattedAddress, Location location)
        {
            GooglePlaceId = googlePlaceId;
            FormattedAddress = formattedAddress;
            // See https://docs.microsoft.com/en-us/ef/core/modeling/spatial#longitude-and-latitude
            Point = new Point(x: location.Longitude, y: location.Latitude);
        }

        private Place(string googlePlaceId, string formattedAddress, Point point)
        {
            GooglePlaceId = googlePlaceId;
            FormattedAddress = formattedAddress;
            Point = point;
        }

        public string GooglePlaceId { get; private set; }
        public string FormattedAddress { get; private set; }
        public Point Point { get; private set; }

        public Location Location => new Location
        {
            Latitude = Point.Y,
            Longitude = Point.X,
        };

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }
    }
}