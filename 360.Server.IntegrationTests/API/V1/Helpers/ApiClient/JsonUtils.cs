using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Helpers.ApiClient
{
    internal static class JsonUtils
    {
        public static async Task<T?> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        public static StringContent MakeJsonStringContent(object request)
        {
            var content = JsonSerializer.Serialize(request);

            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}