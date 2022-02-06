using _360o.Server.Api.V1.Organizations.Model;
using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        public Store(Guid organizationId)
        {
            OrganizationId = Guard.Against.NullOrEmpty(organizationId, nameof(organizationId));
        }

        private Store()
        {
        }

        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; }

        public Place Place { get; private set; }

        public List<Item> Items { get; private set; } = new List<Item>();
        public List<Offer> Offers { get; private set; } = new List<Offer>();

        public void SetPlace(Place place)
        {
            Guard.Against.NullOrWhiteSpace(place.GooglePlaceId, nameof(place.GooglePlaceId));
            Guard.Against.NullOrWhiteSpace(place.FormattedAddress, nameof(place.FormattedAddress));
            // See https://docs.mapbox.com/help/glossary/lat-lon/#:~:text=Latitude%20and%20longitude%20are%20a,180%20to%20180%20for%20longitude
            Guard.Against.OutOfRange(place.Location.Latitude, nameof(place.Location.Latitude), -90, 90);
            Guard.Against.OutOfRange(place.Location.Longitude, nameof(place.Location.Longitude), -180, 180);

            Place = place;
        }
    }
}