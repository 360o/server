using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class OfferItem : BaseEntity
    {
        public OfferItem(Guid itemId)
        {
            ItemId = Guard.Against.NullOrEmpty(itemId, nameof(itemId));
        }

        private OfferItem()
        {
        }

        public int? Quantity { get; private set; }

        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }

        public Guid OfferId { get; private set; }
        public Offer Offer { get; private set; }

        public void SetQuantity(int quantity)
        {
            Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        }
    }
}