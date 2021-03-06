using _360o.Server.Api.V1.Stores.Model;

namespace _360.Server.UnitTests.Api.V1.Stores
{
    internal static class Generator
    {
        public static Store MakeRandomStore()
        {
            var organizationId = Fakers.EnglishFaker.Random.Uuid();
            var place = MakeRandomPlace();

            return new Store(organizationId, place);
        }

        public static Place MakeRandomPlace()
        {
            return new Place(
                Fakers.EnglishFaker.Random.String(),
                Fakers.EnglishFaker.Address.FullAddress(),
                MakeRandomLocation()
                );
        }

        public static Location MakeRandomLocation()
        {
            return new Location(Fakers.EnglishFaker.Address.Latitude(), Fakers.EnglishFaker.Address.Longitude());
        }

        public static Item MakeRandomItem()
        {
            var storeId = Fakers.EnglishFaker.Random.Uuid();

            return new Item(storeId);
        }

        public static MoneyValue MakeRandomMoneyValue()
        {
            var amount = Fakers.EnglishFaker.Random.Decimal(0, 100);
            var currencyCode = Fakers.EnglishFaker.PickRandom<Iso4217CurrencyCode>();

            return new MoneyValue(amount, currencyCode);
        }

        public static Offer MakeRandomOffer()
        {
            var storeId = Fakers.EnglishFaker.Random.Uuid();

            return new Offer(storeId);
        }

        public static OfferItem MakeRandomOfferItem()
        {
            var itemId = Fakers.EnglishFaker.Random.Uuid();
            var quantity = Fakers.EnglishFaker.Random.Int(min: 1);

            return new OfferItem(itemId, quantity);
        }
    }
}