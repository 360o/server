using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    internal class Auth0UsernamePasswordAuthService : IAuthService
    {
        private readonly string _auth0Domain;
        private readonly string _auth0ClientId;
        private readonly string _auth0ClientSecret;
        private readonly string _auth0Audience;
        private readonly string _username;
        private readonly string _password;

        private string? _accessToken;
        private DateTimeOffset? _expiresIn;

        public Auth0UsernamePasswordAuthService(string auth0Domain, string auth0ClientId, string auth0ClientSecret, string auth0Audience, string username, string password)
        {
            _auth0Domain = auth0Domain;
            _auth0Audience = auth0Audience;
            _auth0ClientId = auth0ClientId;
            _auth0ClientSecret = auth0ClientSecret;
            _username = username;
            _password = password;
        }

        public async Task<string> GetAccessToken()
        {
            if (_accessToken != null && _expiresIn.HasValue && DateTimeOffset.UtcNow < _expiresIn.Value)
            {
                return _accessToken;
            }

            var uri = $"https://{_auth0Domain}/oauth/token";
            var request = new List<KeyValuePair<string, string>>();
            request.Add(new KeyValuePair<string, string>("grant_type", "password"));
            request.Add(new KeyValuePair<string, string>("client_id", _auth0ClientId));
            request.Add(new KeyValuePair<string, string>("client_secret", _auth0ClientSecret));
            request.Add(new KeyValuePair<string, string>("audience", _auth0Audience));
            request.Add(new KeyValuePair<string, string>("username", _username));
            request.Add(new KeyValuePair<string, string>("password", _password));
            using var content = new FormUrlEncodedContent(request);
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<Auth0Token>(responseContent);

            _accessToken = token.access_token;
            _expiresIn = DateTimeOffset.UtcNow.AddSeconds(token.expires_in);

            return _accessToken;
        }

        private record struct Auth0Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }
    }
}
