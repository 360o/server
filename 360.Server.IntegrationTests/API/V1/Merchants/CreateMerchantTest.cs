using _360o.Server.API.V1.Merchants.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _360.Server.IntegrationTests.API.V1.Merchants
{
    [TestClass]
    public class CreateMerchantTest
    {
        private readonly MerchantsHelper _merchantsHelper;

        public CreateMerchantTest()
        {
            _merchantsHelper = new MerchantsHelper(new AccessTokenHelper());
        }

        [TestMethod]
        public async Task GivenValidRequestShouldReturnCreated()
        {
            var request = new CreateMerchantRequest
            {
                DisplayName = "Marcus' Pizza Place",
                EnglishShortDescription = "The best pizza in Quebec",
                EnglishLongDescription = "We are 15 years in the market, serving the best pizza in the province",
                EnglishCategories = new HashSet<string>()
                {
                    "pizza",
                    "food",
                    "delivery"
                },
                FrenchShortDescription = "La meilleure pizza au Québec",
                FrenchLongDescription = "Nous sommes 15 ans sur le marché, servant la meilleure pizza de la province",
                FrenchCategories = new HashSet<string>()
                {
                    "pizza",
                    "aliments",
                    "livraison"
                },
                Places = new HashSet<CreateMerchantPlace>()
                {
                    new CreateMerchantPlace()
                    {
                        GooglePlaceId = "ChIJxwWOkXmWuEwR5D8HmD922-M",
                        FormattedAddress = "836 Av. Turnbull, Québec, QC G1R 2X4, Canada",
                        Location = new Location(latitude: 46.807441197347366, longitude: -71.22516500170805)
                    },
                    new CreateMerchantPlace()
                    {
                        GooglePlaceId = "ChIJAQCQd3GWuEwRZiJMzkwDFvc",
                        FormattedAddress = "330 Rue Saint-Vallier E suite 025, Québec, QC G1K 9C5, Canada",
                        Location = new Location(latitude: 46.8104932, longitude: -71.2317815)
                    }
                }
            };

            var result = await _merchantsHelper.CreateMerchantAsync(request);

            Assert.IsTrue(Guid.TryParse(result.Id.ToString(), out var _));
            Assert.AreEqual(request.DisplayName, result.DisplayName);
            Assert.AreEqual(request.EnglishShortDescription, result.EnglishShortDescription);
            Assert.AreEqual(request.EnglishLongDescription, result.EnglishLongDescription);
            Assert.IsTrue(result.EnglishCategories.SetEquals(request.EnglishCategories));
            Assert.AreEqual(request.FrenchLongDescription, result.FrenchLongDescription);
            Assert.AreEqual(request.FrenchLongDescription, result.FrenchLongDescription);
            Assert.IsTrue(result.FrenchCategories.SetEquals(request.FrenchCategories));

            Assert.AreEqual(2, result.Places.Count);

            var requestPlace1 = request.Places.First(p => p.GooglePlaceId == "ChIJxwWOkXmWuEwR5D8HmD922-M");
            var resultPlace1 = result.Places.First(p => p.GooglePlaceId == "ChIJxwWOkXmWuEwR5D8HmD922-M");
            Assert.IsTrue(Guid.TryParse(resultPlace1.Id.ToString(), out var _));
            Assert.AreEqual(requestPlace1.FormattedAddress, resultPlace1.FormattedAddress);
            Assert.AreEqual(requestPlace1.Location.Latitude, resultPlace1.Location.Latitude);
            Assert.AreEqual(requestPlace1.Location.Longitude, resultPlace1.Location.Longitude);

            var requestPlace2 = request.Places.First(p => p.GooglePlaceId == "ChIJAQCQd3GWuEwRZiJMzkwDFvc");
            var resultPlace2 = result.Places.First(p => p.GooglePlaceId == "ChIJAQCQd3GWuEwRZiJMzkwDFvc");
            Assert.IsTrue(Guid.TryParse(resultPlace2.Id.ToString(), out var _));
            Assert.AreEqual(requestPlace2.FormattedAddress, resultPlace2.FormattedAddress);
            Assert.AreEqual(requestPlace2.Location.Latitude, resultPlace2.Location.Latitude);
            Assert.AreEqual(requestPlace2.Location.Longitude, resultPlace2.Location.Longitude);

            var merchant = await _merchantsHelper.GetMerchantByIdAsync(result.Id);
            Assert.AreEqual(result, merchant);
        }
    }
}
