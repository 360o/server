using Bogus;

namespace _360.Server.UnitTests.Api.V1
{
    internal static class Fakers
    {
        public static Faker EnglishFaker => new Faker();
        public static Faker FrenchFaker => new Faker("fr");
    }
}