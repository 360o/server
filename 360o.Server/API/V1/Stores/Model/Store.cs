using _360o.Server.API.V1.Organizations.Model;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        private Store()
        {
        }

        public Store(Guid organizationId, Place place)
        {
            OrganizationId = organizationId;
            Place = place;
        }

        public Guid OrganizationId { get; private set; }
        public Organization Organization { get; private set; }

        public Place Place { get; private set; }
    }
}

