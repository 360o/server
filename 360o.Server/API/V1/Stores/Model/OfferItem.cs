using Ardalis.GuardClauses;

namespace _360o.Server.Api.V1.Stores.Model
{
    public class OfferItem : BaseEntity
    {
        private int _quantity;

        public OfferItem(Guid itemId, int quantity)
        {
            ItemId = Guard.Against.NullOrEmpty(itemId, nameof(itemId));
            Quantity = quantity;
        }

        private OfferItem()
        {
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = Guard.Against.NegativeOrZero(value, nameof(value));
            }
        }

        public Guid ItemId { get; private set; }
        public Item Item { get; private set; }

        public Guid OfferId { get; private set; }
        public Offer Offer { get; private set; }
    }
}