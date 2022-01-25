using System.Text.Json.Serialization;
using _360o.Server.API.V1.Merchants.Model;
using NetTopologySuite.Geometries;

namespace _360o.Server.Merchants.API.V1.Model
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
            Point = new Point(location.Longitude, location.Latitude);
        }

        public string GooglePlaceId { get; private set; }

        public string FormattedAddress { get; private set; }

        public Point Point { get; private set; }

        public Location Location => new Location(Point.Y, Point.X);

        public Guid MerchantId { get; private set; }
        [JsonIgnore]
        public Merchant Merchant { get; private set; }
    }
}

