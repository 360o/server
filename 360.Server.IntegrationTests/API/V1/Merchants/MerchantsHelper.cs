using _360o.Server.API.V1.Merchants.Controllers.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Merchants
{
    internal class MerchantsHelper
    {
        private readonly AccessTokenHelper _accessTokenHelper;

        public MerchantsHelper(AccessTokenHelper accessTokenHelper)
        {
            _accessTokenHelper = accessTokenHelper;
        }

        public async Task<MerchantDTO> CreateMerchantAsync(CreateMerchantRequest request)
        {
            var token = await _accessTokenHelper.GetRegularUserToken();

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient(token.access_token).PostAsync("/api/v1/merchants", jsonContent);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<MerchantDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<MerchantDTO> GetMerchantByIdAsync(Guid id)
        {
            var uri = $"/api/v1/merchants/{id}";

            var response = await ProgramTest.NewClient().GetAsync(uri);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<MerchantDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
    }
}
