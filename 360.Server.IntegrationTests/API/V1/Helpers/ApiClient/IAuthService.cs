using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Helpers.ApiClient
{
    public interface IAuthService
    {
        Task<string> GetAccessToken();
    }
}
