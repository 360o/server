using _360o.Server.API.V1.Stores.Controllers.DTOs;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    public class StoresHelper
    {
        private readonly AccessTokenHelper _accessTokenHelper;

        public StoresHelper(AccessTokenHelper accessTokenHelper)
        {
            _accessTokenHelper = accessTokenHelper;
        }

        public async Task<HttpResponseMessage> CreateStoreAsync(CreateStoreRequest request, StoreUser user = StoreUser.One)
        {
            string accessToken = string.Empty;

            if (user == StoreUser.One)
            {
                accessToken = await _accessTokenHelper.GetRegularUserToken1();
            }
            else if (user == StoreUser.Two)
            {
                accessToken = await _accessTokenHelper.GetRegularUserToken2();
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient(accessToken).PostAsync("/api/v1/stores", jsonContent);

            return response;
        }

        public async Task<StoreDTO> CreateStoreAndDeserializeAsync(CreateStoreRequest request, StoreUser user = StoreUser.One)
        {
            var response = await CreateStoreAsync(request, user);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }

        public async Task<HttpResponseMessage> GetStoreByIdAsync(Guid id)
        {
            var uri = $"/api/v1/stores/{id}";

            var response = await ProgramTest.NewClient().GetAsync(uri);

            return response;
        }

        public async Task<HttpResponseMessage> ListStoresAsync(ListStoresRequest request)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (request.Query != null)
            {
                queryString.Add("query", request.Query);
            }

            if (request.Latitude != null)
            {
                queryString.Add("latitude", request.Latitude.ToString());
            }

            if (request.Longitude != null)
            {
                queryString.Add("longitude", request.Longitude.ToString());
            }

            if (request.Radius != null)
            {
                queryString.Add("radius", request.Radius.ToString());
            }

            var uri = "/api/v1/stores";

            var qs = queryString.ToString();

            if (!string.IsNullOrEmpty(qs))
            {
                uri = $"{uri}?{qs}";
            }

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

        public void AssertStoresEqual(StoreDTO expected, StoreDTO actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.DisplayName, actual.DisplayName);
            Assert.AreEqual(expected.EnglishShortDescription, actual.EnglishShortDescription);
            Assert.AreEqual(expected.EnglishLongDescription, actual.EnglishLongDescription);
            Assert.IsTrue(actual.EnglishCategories.SetEquals(expected.EnglishCategories));
            Assert.AreEqual(expected.FrenchShortDescription, actual.FrenchShortDescription);
            Assert.AreEqual(expected.FrenchLongDescription, actual.FrenchLongDescription);
            Assert.IsTrue(actual.FrenchCategories.SetEquals(expected.FrenchCategories));
            Assert.AreEqual(expected.Place, actual.Place);
        }
    }

    public enum StoreUser
    {
        One,
        Two,
    }
}
