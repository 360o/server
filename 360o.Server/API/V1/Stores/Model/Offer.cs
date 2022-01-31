namespace _360o.Server.API.V1.Stores.Model
{
    public class Offer : BaseEntity
    {
        private readonly List<OfferItem> _offerItems = new List<OfferItem>();

        private Offer()
        {
        }

        public Offer(Guid storeId, string name)
        {
            StoreId = storeId;
            Name = name;
        }

        public string Name { get; private set; }
        public IReadOnlyList<OfferItem> OfferItems => _offerItems;

        public MoneyValue Discount { get; private set; }

        public void AddItem(OfferItem offerItem)
        {
            if (!_offerItems.Any(i => i.Id == offerItem.Id))
            {
                _offerItems.Add(offerItem);
                return;
            }

            var existingOfferItem = OfferItems.First(i => i.Id == offerItem.Id);
            _offerItems.Remove(existingOfferItem);
            _offerItems.Add(offerItem);
        }

        public void RemoveItem(OfferItem item)
        {
            if (!_offerItems.Any(i => i.Id == item.Id))
            {
                return;
            }

            var existingOfferItem = OfferItems.First(i => i.Id == item.Id);
            _offerItems.Remove(existingOfferItem);
        }

        public void AddDiscount(MoneyValue discount)
        {
            Discount = discount;
        }

        public Guid StoreId { get; private set; }
        public Store Store { get; private set; }
    }
}
