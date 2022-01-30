using System;
using System.Net.Http;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    public class ApiClient
    {
        public ApiClient(string auth0Domain, string auth0ClientId, string auth0ClientSecret, string auth0Audience, string username, string password)
        {
            var authService = new Auth0UsernamePasswordAuthService(auth0Domain, auth0ClientId, auth0ClientSecret, auth0Audience, username, password);
            Organizations = new OrganizationsService(authService);
            Stores = new StoresService(authService);
        }

        public OrganizationsService Organizations { get; private set; }
        public StoresService Stores { get; private set; }
    }
}
