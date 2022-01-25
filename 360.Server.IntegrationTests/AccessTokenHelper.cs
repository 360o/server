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

        private Token? _regularUserToken;

        public AccessTokenHelper()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<AccessTokenHelper>();

            _configuration = builder.Build();
        }

        public async Task<Token> GetRegularUserToken()
        {
            if (_regularUserToken.HasValue)
            {
                return _regularUserToken.Value;
            }

            using var client = new HttpClient();
            var uri = $"https://{_configuration["Auth0:Domain"]}/oauth/token";
            var request = new List<KeyValuePair<string, string>>();
            request.Add(new KeyValuePair<string, string>("grant_type", "password"));
            request.Add(new KeyValuePair<string, string>("client_id", _configuration["Auth0:ClientId"]));
            request.Add(new KeyValuePair<string, string>("client_secret", _configuration["Auth0:ClientSecret"]));
            request.Add(new KeyValuePair<string, string>("audience", _configuration["Auth0:Audience"]));
            request.Add(new KeyValuePair<string, string>("username", "regular-user@example.com"));
            request.Add(new KeyValuePair<string, string>("password", "5T6YrFkp3trI"));
            using var content = new FormUrlEncodedContent(request);
            var response = await client.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            _regularUserToken = JsonSerializer.Deserialize<Token>(responseContent);
            return _regularUserToken.Value;
        }

        public record struct Token
        {
            public string access_token { get; set; }
        }
    }
}
