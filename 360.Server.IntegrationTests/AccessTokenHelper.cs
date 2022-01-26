using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests
{
    public class AccessTokenHelper
    {
        private readonly IConfiguration _configuration;

        private string? _regularUserToken1;

        private string? _regularUserToken2;

        public AccessTokenHelper()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<AccessTokenHelper>();

            _configuration = builder.Build();
        }

        public async Task<string> GetRegularUserToken1()
        {
            if (_regularUserToken1 != null)
            {
                return _regularUserToken1;
            }

            _regularUserToken1 = await GetAccessToken("regular-user-1@example.com", "37kV7n5ROU9z");

            return _regularUserToken1;
        }

        public async Task<string> GetRegularUserToken2()
        {
            if (_regularUserToken2 != null)
            {
                return _regularUserToken2;
            }

            _regularUserToken2 = await GetAccessToken("regular-user-2@example.com", "wMa7O70jGwHn");

            return _regularUserToken2;
        }

        private async Task<string> GetAccessToken(string username, string password)
        {
            using var client = new HttpClient();
            var uri = $"https://{_configuration["Auth0:Domain"]}/oauth/token";
            var request = new List<KeyValuePair<string, string>>();
            request.Add(new KeyValuePair<string, string>("grant_type", "password"));
            request.Add(new KeyValuePair<string, string>("client_id", _configuration["Auth0:ClientId"]));
            request.Add(new KeyValuePair<string, string>("client_secret", _configuration["Auth0:ClientSecret"]));
            request.Add(new KeyValuePair<string, string>("audience", _configuration["Auth0:Audience"]));
            request.Add(new KeyValuePair<string, string>("username", username));
            request.Add(new KeyValuePair<string, string>("password", password));
            using var content = new FormUrlEncodedContent(request);
            var response = await client.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<Token>(responseContent);
            return token.access_token;
        }

        private record struct Token
        {
            public string access_token { get; set; }
        }
    }
}
