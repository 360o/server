namespace _360o.Server.API.V1.Stores.Services
{
    public readonly record struct CreateOfferInput
    {
        public CreateOfferInput(Guid storeId)
        {
            StoreId = storeId;
        }

        public Guid StoreId { get; }
    }
}
