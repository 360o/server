using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Bogus;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    public class StoresHelper
    {
        private readonly AccessTokenHelper _accessTokenHelper;

        private string? _regularUserAccessToken;

        public StoresHelper(AccessTokenHelper accessTokenHelper)
        {
            _accessTokenHelper = accessTokenHelper;
        }

        public async Task<HttpResponseMessage> CreateStoreAsync(CreateStoreRequest request)
        {
            if (_regularUserAccessToken == null)
            {
                _regularUserAccessToken = await _accessTokenHelper.GetRegularUserToken();
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient(_regularUserAccessToken).PostAsync("/api/v1/stores", jsonContent);

            return response;
        }

        public async Task<HttpResponseMessage> GetStoreByIdAsync(Guid id)
        {
            var uri = $"/api/v1/stores/{id}";

            var response = await ProgramTest.NewClient().GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> DeleteStoreByIdAsync(Guid id)
        {
            var uri = $"/api/v1/stores/{id}";

            var response = await ProgramTest.NewClient().DeleteAsync(uri);

            return response;
        }

        public CreateStoreRequest MakeRandomCreateMerchantRequest()
        {
            var englishFaker = new Faker();
            var frenchFaker = new Faker(locale: "fr");

            var request = new CreateStoreRequest
            {
                DisplayName = englishFaker.Company.CompanyName(),
                EnglishShortDescription = englishFaker.Company.CatchPhrase(),
                EnglishLongDescription = englishFaker.Commerce.ProductDescription(),
                EnglishCategories = englishFaker.Commerce.Categories(englishFaker.Random.Int(0, 5)).ToHashSet(),
                FrenchShortDescription = frenchFaker.Company.CatchPhrase(),
                FrenchLongDescription = frenchFaker.Commerce.ProductDescription(),
                FrenchCategories = frenchFaker.Commerce.Categories(frenchFaker.Random.Int(0, 5)).ToHashSet(),
                Place = new CreateMerchantPlace()
                {
                    GooglePlaceId = englishFaker.Random.Uuid().ToString(),
                    FormattedAddress = englishFaker.Address.FullAddress(),
                    Location = new LocationDTO
                    {
                        Latitude = englishFaker.Address.Latitude(),
                        Longitude = englishFaker.Address.Longitude()
                    }
                },
            };

            return request;
        }
    }
}
