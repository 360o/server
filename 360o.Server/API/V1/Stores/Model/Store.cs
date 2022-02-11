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
                _place = Guard.Against.Null(value, "Place");
            }
        }

        public List<Item> Items { get; private set; } = new List<Item>();
        public List<Offer> Offers { get; private set; } = new List<Offer>();
    }
}