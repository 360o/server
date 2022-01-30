using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace _360.Server.IntegrationTests
{
    [TestClass]
    public class ProgramTest
    {
        private static WebApplicationFactory<Program> _application;

        public static ApiClient ApiClientUser1 { get; private set;}

        public static ApiClient ApiClientUser2 { get; private set; }

        public static HttpClient NewClient()
        {
            return _application.CreateClient();
        }

        public static HttpClient NewClient(string accessToken)
        {
            var client = NewClient();
            client.DefaultRequestHeaders.Add("authorization", $"Bearer {accessToken}");
            return client;
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<ProgramTest>();

            var configuration = builder.Build();

            var auth0Domain = configuration["Auth0:Domain"];
            var auth0ClientId = configuration["Auth0:ClientId"];
            var auth0ClientSecret = configuration["Auth0:ClientSecret"];
            var auth0Audience = configuration["Auth0:Audience"];

            var user1 = new
            {
                username = "regular-user-1@example.com",
                password = "37kV7n5ROU9z"
            };

            var user2 = new
            {
                username = "regular-user-2@example.com",
                password = "wMa7O70jGwHn"
            };

            _application = new WebApplicationFactory<Program>();

            ApiClientUser1 = new ApiClient(
                auth0Domain, 
                auth0ClientId, 
                auth0ClientSecret, 
                auth0Audience,
                user1.username, 
                user1.password);

            ApiClientUser2 = new ApiClient(
                auth0Domain,
                auth0ClientId,
                auth0ClientSecret,
                auth0Audience,
                user2.username,
                user2.password);
        }
    }
}
