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
    }
}