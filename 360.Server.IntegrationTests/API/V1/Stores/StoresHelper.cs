﻿using _360o.Server.API.V1.Stores.Controllers.DTOs;
using _360o.Server.API.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    internal class StoresHelper
    {
        private readonly AccessTokenHelper _accessTokenHelper;

        public StoresHelper(AccessTokenHelper accessTokenHelper)
        {
            _accessTokenHelper = accessTokenHelper;
        }

        public async Task<StoreDTO> CreateMerchantAsync(CreateStoreRequest request)
        {
            var token = await _accessTokenHelper.GetRegularUserToken();

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await ProgramTest.NewClient(token.access_token).PostAsync("/api/v1/stores", jsonContent);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public async Task<StoreDTO> GetMerchantByIdAsync(Guid id)
        {
            var uri = $"/api/v1/stores/{id}";

            var response = await ProgramTest.NewClient().GetAsync(uri);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<StoreDTO>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }

        public CreateStoreRequest MakeRandomCreateMerchantRequest()
        {
            var englishFaker = new Faker();
            var frenchFaker = new Faker("fr_CA");

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
