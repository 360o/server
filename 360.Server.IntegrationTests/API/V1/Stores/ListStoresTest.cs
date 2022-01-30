using _360.Server.IntegrationTests.API.V1.Helpers.ApiClient;
using _360.Server.IntegrationTests.API.V1.Helpers.Generators;
using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Stores
{
    [TestClass]
    public class ListStoresTest
    {
        [TestMethod]
        public async Task GivenNoFilterShouldReturnAllStores()
        {
            var user1Organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();
            var user2Organization = await ProgramTest.ApiClientUser2.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<StoreDTO>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(result);
            var resultUser1Store1 = result.Find(s => s.Id == user1Store1.Id);
            var resultUser1Store2 = result.Find(s => s.Id == user1Store2.Id);
            var resultUser2Store1 = result.Find(s => s.Id == user2Store1.Id);
            Assert.AreEqual(user1Store1, resultUser1Store1);
            Assert.AreEqual(user1Store2, resultUser1Store2);
            Assert.AreEqual(user2Store1, resultUser2Store1);
        }

        [TestMethod]
        public async Task GivenQueryParamShouldReturnStoresWithDisplayNameOrDescriptionsOrCategoriesMatchingTheQuery()
        {
            var myQuery = "amigurumi";

            var user1Organization1Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1Organization2Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1Organization3Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1Organization4Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2Organization1Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2Organization2Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2Organization3Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            user1Organization1Request.Name = $"{user1Organization1Request.Name} {myQuery}";
            user1Organization2Request.EnglishShortDescription = $"{user1Organization2Request.EnglishShortDescription} {myQuery}";
            user1Organization3Request.EnglishLongDescription = $"{user1Organization3Request.EnglishLongDescription} {myQuery}";
            user1Organization4Request.EnglishCategories.Add(myQuery);
            user2Organization1Request.FrenchShortDescription = $"{user2Organization1Request.FrenchShortDescription} {myQuery}";
            user2Organization2Request.FrenchLongDescription = $"{user2Organization2Request.FrenchLongDescription} {myQuery}";
            user2Organization3Request.FrenchCategories.Add(myQuery);

            var user1Organization1 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization1Request);
            var user1Organization2 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization2Request);
            var user1Organization3 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization3Request);
            var user1Organization4 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization4Request);
            var user2Organization1 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization1Request);
            var user2Organization2 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization2Request);
            var user2Organization3 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization3Request);

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization1.Id);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization2.Id);
            var user1Store3 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization3.Id);
            var user1Store4 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization4.Id);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2Organization1.Id);
            var user2Store2 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2Organization2.Id);
            var user2Store3 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2Organization3.Id);

            var expectedStores = new Dictionary<Guid, StoreDTO>()
            {
                { user1Store1.Id, user1Store1 },
                { user1Store2.Id, user1Store2 },
                { user1Store3.Id, user1Store3 },
                { user1Store4.Id, user1Store4 },
                { user2Store1.Id, user2Store1 },
                { user2Store2.Id, user2Store2 },
                { user2Store3.Id, user2Store3 }
            };

            var stores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Query = myQuery
            });

            Assert.IsNotNull(stores);
            Assert.AreEqual(expectedStores.Count, stores.Count);
            foreach (var store in stores)
            {
                Assert.AreEqual(expectedStores[store.Id], store);
            }
        }

        [TestMethod]
        public async Task GivenLatitudeLongitudeAndRadiusParamsShouldReturnStoresWithinTheRadiusOfTheLocationInMeters()
        {
            var myLocation = new Location(46.8074417, -71.2271805);

            var twoHundredMeters = 200;
            var sixHundredMeters = 600;
            var twoKilometers = 2000;
            var tenKilometers = 10000;

            var storesWithinTwoHundredMeters = new Dictionary<Guid, StoreDTO>();
            var storesWithinSixHundredMeters = new Dictionary<Guid, StoreDTO>();
            var storesWithinTwoKilometers = new Dictionary<Guid, StoreDTO>();
            var storesWithinTenKilometers = new Dictionary<Guid, StoreDTO>();

            var user1Organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();
            var user2Organization = await ProgramTest.ApiClientUser2.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var user1Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization.Id);
            var user1Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization.Id);
            var user1Store3Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization.Id);
            var user1Store4Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization.Id);
            var user2Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization.Id);
            var user2Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization.Id);
            var user2Store3Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization.Id);

            // 200m
            user1Store1Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJmwaUgluXuEwRWZsk59TJKnY",
                FormattedAddress = "249 Rue Saint-Jean, Québec, QC G1R 1N8, Canada",
                Location = new Location(46.8074417, -71.2271805)
            };

            user1Store2Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJmTMHIHqWuEwRPXPWy5zuZNI",
                FormattedAddress = "165 Rue Saint-Jean, Québec, QC G1R 1N4",
                Location = new Location(46.807864, -71.2270421)
            };

            // 600m
            user2Store1Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJVTZiSnmWuEwRiMC1PoeqRFY",
                FormattedAddress = "85 Boulevard René-Lévesque O, Québec, QC G1R 2A3, Canada",
                Location = new Location(46.8048075, -71.2256069)
            };

            user1Store3Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJf4k1oo-WuEwRO-iB-_Asl5Q",
                FormattedAddress = "269 Bd René-Lévesque E, Québec, QC G1R 2B3, Canada",
                Location = new Location(46.8068828, -71.2245926)
            };

            // 2km
            user2Store2Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJO_wtXNqVuEwRBoNhbzRrcZg",
                FormattedAddress = "1 Côte de la Citadelle, Québec, QC G1R 3R2, Canada",
                Location = new Location(46.8079412, -71.2189697)
            };

            // 10km
            user1Store4Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJTWbEbimRuEwRT_YQIJcyOl8",
                FormattedAddress = "3121 Bd Hochelaga, Québec, QC G1W 2P9, Canada",
                Location = new Location(46.7863078, -71.2841527)
            };

            user2Store3Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJp8qY5EaWuEwR_Su2geare9E",
                FormattedAddress = "552 Bd Wilfrid-Hamel, Québec, QC G1M 3E5, Canada",
                Location = new Location(46.8180445, -71.2536773)
            };

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store1Request);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store2Request);
            var user1Store3 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store3Request);
            var user1Store4 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store4Request);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateStoreAndDeserializeAsync(user2Store1Request);
            var user2Store2 = await ProgramTest.ApiClientUser2.Stores.CreateStoreAndDeserializeAsync(user2Store2Request);
            var user2Store3 = await ProgramTest.ApiClientUser2.Stores.CreateStoreAndDeserializeAsync(user2Store3Request);

            storesWithinTwoHundredMeters.Add(user1Store1.Id, user1Store1);
            storesWithinTwoHundredMeters.Add(user1Store2.Id, user1Store2);

            storesWithinTwoHundredMeters.ToList().ForEach(kv => storesWithinSixHundredMeters.Add(kv.Key, kv.Value));
            storesWithinSixHundredMeters.Add(user2Store1.Id, user2Store1);
            storesWithinSixHundredMeters.Add(user1Store3.Id, user1Store3);

            storesWithinSixHundredMeters.ToList().ForEach(kv => storesWithinTwoKilometers.Add(kv.Key, kv.Value));
            storesWithinTwoKilometers.Add(user2Store2.Id, user2Store2);

            storesWithinTwoKilometers.ToList().ForEach(kv => storesWithinTenKilometers.Add(kv.Key, kv.Value));
            storesWithinTenKilometers.Add(user1Store4.Id, user1Store4);
            storesWithinTenKilometers.Add(user2Store3.Id, user2Store3);

            var twoHundredMetersStores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude,
                Radius = twoHundredMeters
            });

            Assert.IsNotNull(twoHundredMetersStores);
            Assert.AreEqual(storesWithinTwoHundredMeters.Count, twoHundredMetersStores.Count);

            foreach (var store in twoHundredMetersStores)
            {
                Assert.AreEqual(storesWithinTwoHundredMeters[store.Id], store);
            }

            var sixHundredMetersStore = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude,
                Radius = sixHundredMeters
            });

            Assert.IsNotNull(sixHundredMetersStore);
            Assert.AreEqual(storesWithinSixHundredMeters.Count, sixHundredMetersStore.Count);

            foreach (var store in sixHundredMetersStore)
            {
                Assert.AreEqual(storesWithinSixHundredMeters[store.Id], store);
            }

            var twoKilometersStores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude,
                Radius = twoKilometers
            });

            Assert.IsNotNull(twoKilometersStores);
            Assert.AreEqual(storesWithinTwoKilometers.Count, twoKilometersStores.Count);

            foreach (var store in twoKilometersStores)
            {
                Assert.AreEqual(storesWithinTwoKilometers[store.Id], store);
            }

            var tenKilometersStores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude,
                Radius = tenKilometers
            });

            Assert.IsNotNull(tenKilometersStores);
            Assert.AreEqual(storesWithinTenKilometers.Count, tenKilometersStores.Count);

            foreach (var store in tenKilometersStores)
            {
                Assert.AreEqual(storesWithinTenKilometers[store.Id], store);
            }
        }

        [TestMethod]
        public async Task GivenRadiusAndNoLatitudeAndNoLongitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Radius = faker.Random.Double()
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Latitude"));
            Assert.IsTrue(result.Detail.Contains("Longitude"));
        }

        [TestMethod]
        public async Task GivenLatitudeAndNoRadiusAndNoLongitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = faker.Address.Latitude()
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Radius"));
            Assert.IsTrue(result.Detail.Contains("Longitude"));
        }

        [TestMethod]
        public async Task GivenLongitudeAndNoRadiusAndNoLatitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Longitude = faker.Address.Longitude()
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Radius"));
            Assert.IsTrue(result.Detail.Contains("Latitude"));
        }

        public async Task GivenRadiusAndLatitudeAndNoLongitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = faker.Address.Latitude(),
                Radius = faker.Random.Double()
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Longitude"));
        }

        public async Task GivenRadiusAndLongitudeAndNoLatitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Longitude = faker.Address.Longitude(),
                Radius = faker.Random.Double(),
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Latitude"));
        }

        public async Task GivenLatitudeAndLongitudeAndNoRadiusShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = faker.Address.Latitude(),
                Longitude = faker.Address.Longitude(),
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Detail);
            Assert.IsNotNull(result.Status);
            Assert.AreEqual(ErrorCode.InvalidRequest.ToString(), result.Title);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.Status.Value);
            Assert.IsTrue(result.Detail.Contains("Radius"));
        }

        [TestMethod]
        public async Task GivenQueryAndLatitudeAndLongitudeAndRadiusShouldReturnStoresThatMatchQueryWithinTheRadiusOfTheLocationInMeters()
        {
            var myQuery = "nail";
            var myLocation = new Location(52.2318464, 20.9998793);
            var twoKilometers = 2000;

            var user1Organization1Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1Organization2Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2Organization1Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2Organization2Request = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            user1Organization1Request.Name = "BEAUTY STUDIO SOPHIA";
            user1Organization1Request.EnglishCategories = new HashSet<string>(new[] { "nail salon" });

            user1Organization2Request.Name = "Portobello Pizza & Pasta - kuchnia włoska";

            user2Organization1Request.Name = "Studio Hollywood Nails";

            user2Organization2Request.Name = "Aura Manicure & Pedicure";
            user2Organization2Request.EnglishCategories = new HashSet<string>(new[] { "nail salon" });

            var user1Organization1 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization1Request);
            var user1Organization2 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization2Request);
            var user2Organization1 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization1Request);
            var user2Organization2 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization2Request);

            var user1Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization1.Id);
            var user1Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization2.Id);
            var user2Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization1.Id);
            var user2Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization2.Id);

            user1Store1Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJ_RYTY4nNHkcRl6fA-1FcRRE",
                FormattedAddress = "Chmielna 106, 00-801 Warszawa, Poland",
                Location = new Location(52.2288783, 20.9963373)
            };

            user1Store2Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJSYbSBiXNHkcRPVTeBjMEWBE",
                FormattedAddress = "al. Jana Pawła II 12, 00-001 Warszawa, Poland",
                Location = new Location(52.2318464, 20.9998793)
            };

            user2Store1Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJkb3oh_TMHkcRvwTuCI-g9us",
                FormattedAddress = "Świętokrzyska 30, 00-116 Warszawa, Poland",
                Location = new Location(52.2349257, 21.0032608)
            };


            user2Store2Request.Place = new CreateStoreRequestPlace
            {
                GooglePlaceId = "ChIJAWdPJtEyGUcRFfnT7IcNGFg",
                FormattedAddress = "Domaniewska 22A, 02-672 Warszawa, Poland",
                Location = new Location(52.1881715, 21.0114313)
            };

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store1Request);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateStoreAndDeserializeAsync(user1Store2Request);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateStoreAndDeserializeAsync(user2Store1Request);
            var user2Store2 = await ProgramTest.ApiClientUser2.Stores.CreateStoreAndDeserializeAsync(user2Store2Request);

            var expectedStores = new Dictionary<Guid, StoreDTO>()
            {
                { user1Store1.Id, user1Store1 },
                { user2Store1.Id, user2Store1 },
            };

            var stores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Query = myQuery,
                Latitude = myLocation.Latitude,
                Longitude = myLocation.Longitude,
                Radius = twoKilometers
            });

            Assert.AreEqual(expectedStores.Count, stores.Count);

            foreach (var store in stores)
            {
                Assert.AreEqual(expectedStores[store.Id], store);
            }
        }
    }
}
