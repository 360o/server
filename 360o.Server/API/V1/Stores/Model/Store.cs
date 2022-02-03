using _360o.Server.API.V1.Organizations.Model;

namespace _360o.Server.API.V1.Stores.Model
{
    public class Store : BaseEntity
    {
        public Store(Guid organizationId, Place place)
        {
            OrganizationId = organizationId;
            Place = place;
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
            Place = place;
        }

        public void AddOffer(Offer offer)
        {
            if (!Offers.Any(o => o.Id == offer.Id))
            {
                Offers.Add(offer);
                return;
            }

            var existingOffer = Offers.First(o => o.Id == offer.Id);
            Offers.Remove(existingOffer);
            Offers.Add(offer);
        }

        public void RemoveOffer(Guid offerId)
        {
            if (!Offers.Any(i => i.Id == offerId))
            {
                return;
            }

            var existingOffer = Offers.First(i => i.Id == offerId);
            Offers.Remove(existingOffer);
        }
    }
}