using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    static class Utils
    {
        public static async Task<T?> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}
