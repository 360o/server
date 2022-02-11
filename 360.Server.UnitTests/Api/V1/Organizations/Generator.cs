using _360o.Server.Api.V1.Organizations.Model;

namespace _360.Server.UnitTests.Api.V1.Organizations
{
    internal static class Generator
    {
        public static Organization MakeRandomOrganization()
        {
            var userId = Fakers.EnglishFaker.Internet.UserName();
            var name = Fakers.EnglishFaker.Company.CompanyName();

            return new Organization(userId, name);
        }
    }
}