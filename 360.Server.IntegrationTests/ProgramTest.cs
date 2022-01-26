using _360.Server.IntegrationTests.API.V1.Stores;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace _360.Server.IntegrationTests
{
    [TestClass]
    public class ProgramTest
    {
        private static WebApplicationFactory<Program> _application;

        public static StoresHelper StoresHelper { get; private set; }

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
            _application = new WebApplicationFactory<Program>();

            StoresHelper = new StoresHelper(new AccessTokenHelper());
        }
    }
}
