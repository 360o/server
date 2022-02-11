using _360o.Server.Api.V1.Organizations.Model;
using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        private Place _place;

        public Store(Guid organizationId, Place place)
        {
            OrganizationId = Guard.Against.NullOrEmpty(organizationId, nameof(organizationId));
            Place = place;
        }

        private Store()
        {
        }

        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; }

        public Place Place
        {
            get => _place;
            set
            {
                Guard.Against.NullOrWhiteSpace(value.GooglePlaceId, nameof(value.GooglePlaceId));
                Guard.Against.NullOrWhiteSpace(value.FormattedAddress, nameof(value.FormattedAddress));
                Guard.Against.Null(value.Location, nameof(value.Location));
                // See https://docs.mapbox.com/help/glossary/lat-lon/#:~:text=Latitude%20and%20longitude%20are%20a,180%20to%20180%20for%20longitude
                Guard.Against.OutOfRange(value.Location.Latitude, nameof(value.Location.Latitude), -90, 90);
                Guard.Against.OutOfRange(value.Location.Longitude, nameof(value.Location.Longitude), -180, 180);

                _place = value;
            }
        }

        public List<Item> Items { get; private set; } = new List<Item>();
        public List<Offer> Offers { get; private set; } = new List<Offer>();
    }
}