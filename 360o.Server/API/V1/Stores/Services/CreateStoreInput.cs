using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateStoreInput
    {
        public CreateStoreInput(Guid organizationId, CreateStoreInputPlace place)
        {
            OrganizationId = organizationId;
            Place = place;
        }

        public Guid OrganizationId { get; }
        public CreateStoreInputPlace Place { get; }

        public readonly record struct CreateStoreInputPlace
        {
            public CreateStoreInputPlace(string googlePlaceId, string formattedAddress, Location location)
            {
                GooglePlaceId = googlePlaceId;
                FormattedAddress = formattedAddress;
                Location = location;
            }

            public string GooglePlaceId { get; }
            public string FormattedAddress { get; }
            public Location Location { get; }
        }
    }
}
