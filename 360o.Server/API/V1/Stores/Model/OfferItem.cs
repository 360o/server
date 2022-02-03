namespace _360o.Server.API.V1.Stores.Model
{
    public class OfferItem : BaseEntity
    {
        public OfferItem(Guid offerId, Guid itemId, int quantity = 1)
        {
            OfferId = offerId;
            ItemId = itemId;
            Quantity = quantity;
        }

        private OfferItem()
        {
        }

        public int Quantity { get; private set; } = 1;

        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }

        public Guid OfferId { get; private set; }
        public Offer Offer { get; private set; }
    }
}