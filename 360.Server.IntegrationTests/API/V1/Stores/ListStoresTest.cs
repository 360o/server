using _360.Server.IntegrationTests.Api.V1.Helpers;
using _360.Server.IntegrationTests.Api.V1.Helpers.Generators;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.Api.V1.Stores
{
    [TestClass]
    public class ListStoresTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public async Task GivenNoFilterShouldReturnAllStores()
        {
            var user1Organization = await ProgramTest.ApiClientUser1.Organizations.CreateRandomOrganizationAndDeserializeAsync();
            var user2Organization = await ProgramTest.ApiClientUser2.Organizations.CreateRandomOrganizationAndDeserializeAsync();

            var user1Store1 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user1Store2 = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1Organization.Id);
            var user2Store1 = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2Organization.Id);

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
            CustomAssertions.AssertSerializeToSameJson(user1Store1, resultUser1Store1);
            CustomAssertions.AssertSerializeToSameJson(user1Store2, resultUser1Store2);
            CustomAssertions.AssertSerializeToSameJson(user2Store1, resultUser2Store1);
        }

        [TestMethod]
        public async Task GivenQueryParamShouldReturnStoresNameOrDescriptionsOrCategoriesOrItemsMatchingTheQuery()
        {
            var myQuery = "amigurumi";

            var user1OrganizationWithNameRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1OrganizationWithEnglishShortDescriptionRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1OrganizationWithEnglishLongDescriptionRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1OrganizationWithEnglishCategoriesRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user1OrganizationWithoutQueryRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            var user2OrganizationWithFrenchShortDescriptionRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2OrganizationWithFrenchLongDescriptionRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2OrganizationWithFrenchCategoriesRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();
            var user2OrganizationWithoutQueryRequest = RequestsGenerator.MakeRandomCreateOrganizationRequest();

            user1OrganizationWithNameRequest = user1OrganizationWithNameRequest with { Name = $"{user1OrganizationWithNameRequest.Name} {myQuery}" };
            user1OrganizationWithEnglishShortDescriptionRequest = user1OrganizationWithNameRequest with { EnglishShortDescription = $"{user1OrganizationWithEnglishShortDescriptionRequest.EnglishShortDescription} {myQuery}" };
            user1OrganizationWithEnglishLongDescriptionRequest = user1OrganizationWithNameRequest with { EnglishLongDescription = $"{user1OrganizationWithEnglishLongDescriptionRequest.EnglishLongDescription} {myQuery}" };
            user1OrganizationWithEnglishCategoriesRequest.EnglishCategories.Add(myQuery);

            user2OrganizationWithFrenchShortDescriptionRequest = user1OrganizationWithNameRequest with { FrenchShortDescription = $"{user2OrganizationWithFrenchShortDescriptionRequest.FrenchShortDescription} {myQuery}" };
            user2OrganizationWithFrenchLongDescriptionRequest = user1OrganizationWithNameRequest with { FrenchLongDescription = $"{user2OrganizationWithFrenchLongDescriptionRequest.FrenchLongDescription} {myQuery}" };
            user2OrganizationWithFrenchCategoriesRequest.FrenchCategories.Add(myQuery);

            var user1OrganizationWithName = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1OrganizationWithNameRequest);
            var user1OrganizationWithEnglishShortDescription = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1OrganizationWithEnglishShortDescriptionRequest);
            var user1OrganizationWithEnglishLongDescription = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1OrganizationWithEnglishLongDescriptionRequest);
            var user1OrganizationWithEnglishCategories = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1OrganizationWithEnglishCategoriesRequest);
            var user1OrganizationWithoutQuery = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1OrganizationWithoutQueryRequest);

            var user2OrganizationWithFrenchShortDescription = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2OrganizationWithFrenchShortDescriptionRequest);
            var user2OrganizationWithFrenchLongDescription = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2OrganizationWithFrenchLongDescriptionRequest);
            var user2OrganizationWithFrenchCategories = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2OrganizationWithFrenchCategoriesRequest);
            var user2OrganizationWithoutQuery = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2OrganizationWithoutQueryRequest);

            var user1StoreWithName = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithName.Id);
            var user1StoreWithEnglishShortDescription = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithEnglishShortDescription.Id);
            var user1StoreWithEnglishLongDescription = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithEnglishLongDescription.Id);
            var user1StoreWithEnglishCategories = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithEnglishCategories.Id);
            var user1StoreWithItemWithEnglishName = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithoutQuery.Id);
            var user1StoreWithItemWithEnglishDescription = await ProgramTest.ApiClientUser1.Stores.CreateRandomStoreAndDeserializeAsync(user1OrganizationWithoutQuery.Id);

            var user2StoreWithFrenchShortDescription = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2OrganizationWithFrenchShortDescription.Id);
            var user2StoreWithFrenchLongDescription = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2OrganizationWithFrenchLongDescription.Id);
            var user2StoreWithFrenchCategories = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2OrganizationWithFrenchCategories.Id);
            var user2StoreWithItemWithFrenchName = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2OrganizationWithoutQuery.Id);
            var user2StoreWithItemWithFrenchDescription = await ProgramTest.ApiClientUser2.Stores.CreateRandomStoreAndDeserializeAsync(user2OrganizationWithoutQuery.Id);

            var user1ItemWithEnglishNameRequest = RequestsGenerator.MakeRandomCreateItemRequest();
            user1ItemWithEnglishNameRequest = user1ItemWithEnglishNameRequest with { EnglishName = $"{user1ItemWithEnglishNameRequest.EnglishName} {myQuery}" };

            var user1ItemWithEnglishDescriptionRequest = RequestsGenerator.MakeRandomCreateItemRequest();
            user1ItemWithEnglishDescriptionRequest = user1ItemWithEnglishDescriptionRequest with { EnglishDescription = $"{user1ItemWithEnglishDescriptionRequest.EnglishDescription} {myQuery}" };

            var user2ItemWithFrenchNameRequest = RequestsGenerator.MakeRandomCreateItemRequest();
            user2ItemWithFrenchNameRequest = user2ItemWithFrenchNameRequest with { FrenchName = $"{user2ItemWithFrenchNameRequest.FrenchName} {myQuery}" };

            var user2ItemWithFrenchDescriptionRequest = RequestsGenerator.MakeRandomCreateItemRequest();
            user2ItemWithFrenchDescriptionRequest = user2ItemWithFrenchDescriptionRequest with { FrenchDescription = $"{user2ItemWithFrenchDescriptionRequest.FrenchDescription} {myQuery}" };

            await ProgramTest.ApiClientUser1.Stores.CreateItemAsync(user1StoreWithItemWithEnglishName.Id, user1ItemWithEnglishNameRequest);
            await ProgramTest.ApiClientUser1.Stores.CreateItemAsync(user1StoreWithItemWithEnglishDescription.Id, user1ItemWithEnglishDescriptionRequest);
            await ProgramTest.ApiClientUser2.Stores.CreateItemAsync(user2StoreWithItemWithFrenchName.Id, user2ItemWithFrenchNameRequest);
            await ProgramTest.ApiClientUser2.Stores.CreateItemAsync(user2StoreWithItemWithFrenchDescription.Id, user2ItemWithFrenchDescriptionRequest);

            var stores = await ProgramTest.ApiClientUser1.Stores.ListStoresAndDeserializeAsync(new ListStoresRequest
            {
                Query = myQuery
            });

            var storesDict = stores.ToDictionary(s => s.Id, s => s);

            Assert.IsNotNull(stores);

            CustomAssertions.AssertSerializeToSameJson(user1StoreWithName, storesDict[user1StoreWithName.Id]);
            CustomAssertions.AssertSerializeToSameJson(user1StoreWithEnglishShortDescription, storesDict[user1StoreWithEnglishShortDescription.Id]);
            CustomAssertions.AssertSerializeToSameJson(user1StoreWithEnglishLongDescription, storesDict[user1StoreWithEnglishLongDescription.Id]);
            CustomAssertions.AssertSerializeToSameJson(user1StoreWithEnglishCategories, storesDict[user1StoreWithEnglishCategories.Id]);
            CustomAssertions.AssertSerializeToSameJson(user1StoreWithItemWithEnglishName, storesDict[user1StoreWithItemWithEnglishName.Id]);
            CustomAssertions.AssertSerializeToSameJson(user1StoreWithItemWithEnglishDescription, storesDict[user1StoreWithItemWithEnglishDescription.Id]);

            CustomAssertions.AssertSerializeToSameJson(user2StoreWithFrenchShortDescription, storesDict[user2StoreWithFrenchShortDescription.Id]);
            CustomAssertions.AssertSerializeToSameJson(user2StoreWithFrenchLongDescription, storesDict[user2StoreWithFrenchLongDescription.Id]);
            CustomAssertions.AssertSerializeToSameJson(user2StoreWithFrenchCategories, storesDict[user2StoreWithFrenchCategories.Id]);
            CustomAssertions.AssertSerializeToSameJson(user2StoreWithItemWithFrenchName, storesDict[user2StoreWithItemWithFrenchName.Id]);
            CustomAssertions.AssertSerializeToSameJson(user2StoreWithItemWithFrenchDescription, storesDict[user2StoreWithItemWithFrenchDescription.Id]);
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
            user1Store1Request = user1Store1Request with
            {
                Place = new PlaceDTO("ChIJmwaUgluXuEwRWZsk59TJKnY", "249 Rue Saint-Jean, Québec, QC G1R 1N8, Canada", new LocationDTO(46.8074417, -71.2271805))
            };

            user1Store2Request = user1Store2Request with
            {
                Place = new PlaceDTO("ChIJmTMHIHqWuEwRPXPWy5zuZNI", "165 Rue Saint-Jean, Québec, QC G1R 1N4", new LocationDTO(46.807864, -71.2270421))
            };

            // 600m
            user2Store1Request = user2Store1Request with
            {
                Place = new PlaceDTO("ChIJVTZiSnmWuEwRiMC1PoeqRFY", "85 Boulevard René-Lévesque O, Québec, QC G1R 2A3, Canada", new LocationDTO(46.8048075, -71.2256069))
            };

            user1Store3Request = user1Store3Request with
            {
                Place = new PlaceDTO("ChIJf4k1oo-WuEwRO-iB-_Asl5Q", "269 Bd René-Lévesque E, Québec, QC G1R 2B3, Canada", new LocationDTO(46.8068828, -71.2245926))
            };

            // 2km
            user2Store2Request = user2Store2Request with
            {
                Place = new PlaceDTO("ChIJO_wtXNqVuEwRBoNhbzRrcZg", "1 Côte de la Citadelle, Québec, QC G1R 3R2, Canada", new LocationDTO(46.8079412, -71.2189697))
            };

            // 10km
            user1Store4Request = user1Store4Request with
            {
                Place = new PlaceDTO("ChIJTWbEbimRuEwRT_YQIJcyOl8", "3121 Bd Hochelaga, Québec, QC G1W 2P9, Canada", new LocationDTO(46.7863078, -71.2841527))
            };

            user2Store3Request = user2Store3Request with
            {
                Place = new PlaceDTO("ChIJp8qY5EaWuEwR_Su2geare9E", "552 Bd Wilfrid-Hamel, Québec, QC G1M 3E5, Canada", new LocationDTO(46.8180445, -71.2536773))
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
                CustomAssertions.AssertSerializeToSameJson(storesWithinTwoHundredMeters[store.Id], store);
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
                CustomAssertions.AssertSerializeToSameJson(storesWithinSixHundredMeters[store.Id], store);
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
                CustomAssertions.AssertSerializeToSameJson(storesWithinTwoKilometers[store.Id], store);
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
                CustomAssertions.AssertSerializeToSameJson(storesWithinTenKilometers[store.Id], store);
            }
        }

        [TestMethod]
        public async Task GivenRadiusAndNoLatitudeAndNoLongitudeShouldReturnBadRequest()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Radius = _faker.Random.Double()
            });

            await CustomAssertions.AssertBadRequestAsync(response, new HashSet<string>
            {
                "'Longitude' must not be empty",
                "'Latitude' must not be empty"
            });
        }

        [TestMethod]
        public async Task GivenLatitudeAndNoRadiusAndNoLongitudeShouldReturnBadRequest()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = _faker.Address.Latitude()
            });

            await CustomAssertions.AssertBadRequestAsync(response, new HashSet<string>
            {
                "'Longitude' must not be empty",
                "'Radius' must not be empty"
            });
        }

        [TestMethod]
        public async Task GivenLongitudeAndNoRadiusAndNoLatitudeShouldReturnBadRequest()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Longitude = _faker.Address.Longitude()
            });

            await CustomAssertions.AssertBadRequestAsync(response, new HashSet<string>
            {
                "'Latitude' must not be empty",
                "'Radius' must not be empty"
            });
        }

        [TestMethod]
        public async Task GivenRadiusAndLatitudeAndNoLongitudeShouldReturnBadRequest()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = _faker.Address.Latitude(),
                Radius = _faker.Random.Double()
            });

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Longitude' must not be empty");
        }

        [TestMethod]
        public async Task GivenRadiusAndLongitudeAndNoLatitudeShouldReturnBadRequest()
        {
            var faker = new Faker();

            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Longitude = faker.Address.Longitude(),
                Radius = faker.Random.Double(),
            });

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Latitude' must not be empty");
        }

        [TestMethod]
        public async Task GivenLatitudeAndLongitudeAndNoRadiusShouldReturnBadRequest()
        {
            var response = await ProgramTest.ApiClientUser1.Stores.ListStoresAsync(new ListStoresRequest
            {
                Latitude = _faker.Address.Latitude(),
                Longitude = _faker.Address.Longitude(),
            });

            await CustomAssertions.AssertBadRequestWithProblemDetailsAsync(response, "'Radius' must not be empty");
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

            user1Organization1Request = user1Organization1Request with
            {
                Name = "BEAUTY STUDIO SOPHIA",
                EnglishCategories = new HashSet<string>(new[] { "nail salon" })
            };

            user1Organization2Request = user1Organization2Request with
            {
                Name = "Portobello Pizza & Pasta - kuchnia włoska"
            };

            user2Organization1Request = user2Organization1Request with
            {
                Name = "Studio Hollywood Nails"
            };

            user2Organization2Request = user2Organization2Request with
            {
                Name = "Aura Manicure & Pedicure",
                EnglishCategories = new HashSet<string>(new[] { "nail salon" })
            };

            var user1Organization1 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization1Request);
            var user1Organization2 = await ProgramTest.ApiClientUser1.Organizations.CreateOrganizationAndDeserializeAsync(user1Organization2Request);
            var user2Organization1 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization1Request);
            var user2Organization2 = await ProgramTest.ApiClientUser2.Organizations.CreateOrganizationAndDeserializeAsync(user2Organization2Request);

            var user1Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization1.Id);
            var user1Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user1Organization2.Id);
            var user2Store1Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization1.Id);
            var user2Store2Request = RequestsGenerator.MakeRandomCreateStoreRequest(user2Organization2.Id);

            user1Store1Request = user1Store1Request with
            {
                Place = new PlaceDTO("ChIJ_RYTY4nNHkcRl6fA-1FcRRE", "Chmielna 106, 00-801 Warszawa, Poland", new LocationDTO(52.2288783, 20.9963373))
            };

            user1Store2Request = user1Store2Request with
            {
                Place = new PlaceDTO("ChIJSYbSBiXNHkcRPVTeBjMEWBE", "al. Jana Pawła II 12, 00-001 Warszawa, Poland", new LocationDTO(52.2318464, 20.9998793))
            };

            user2Store1Request = user2Store1Request with
            {
                Place = new PlaceDTO("ChIJkb3oh_TMHkcRvwTuCI-g9us", "Świętokrzyska 30, 00-116 Warszawa, Poland", new LocationDTO(52.2349257, 21.0032608))
            };

            user2Store2Request = user2Store2Request with
            {
                Place = new PlaceDTO("ChIJAWdPJtEyGUcRFfnT7IcNGFg", "Domaniewska 22A, 02-672 Warszawa, Poland", new LocationDTO(52.1881715, 21.0114313))
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

            var storesDict = stores.ToDictionary(s => s.Id, s => s);

            Assert.AreEqual(expectedStores.Count, storesDict.Count);
            CustomAssertions.AssertSerializeToSameJson(user1Store1, storesDict[user1Store1.Id]);
            CustomAssertions.AssertSerializeToSameJson(user2Store1, storesDict[user2Store1.Id]);
        }
    }
}