using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PlaceModel = _360o.Server.Api.V1.Stores.Model.Place;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Place
{
    [TestClass]
    public class PlaceConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnPlace()
        {
            var googlePlaceId = Fakers.EnglishFaker.Random.Uuid().ToString();
            var formattedAddress = Fakers.EnglishFaker.Address.FullAddress();
            var location = Generator.MakeRandomLocation();

            var place = new PlaceModel(
                googlePlaceId,
                formattedAddress,
                location);

            Assert.AreEqual(googlePlaceId, place.GooglePlaceId);
            Assert.AreEqual(formattedAddress, place.FormattedAddress);
            Assert.AreEqual(location, place.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullGooglePlaceIdShouldThrow()
        {
            new PlaceModel(
                null,
                Fakers.EnglishFaker.Address.FullAddress(),
                Generator.MakeRandomLocation());
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceGooglePlaceIdShouldThrow(string googlePlaceId)
        {
            new PlaceModel(
                googlePlaceId,
                Fakers.EnglishFaker.Address.FullAddress(),
                Generator.MakeRandomLocation());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullFormattedAddressShouldThrow()
        {
            new PlaceModel(
                Fakers.EnglishFaker.Random.Uuid().ToString(),
                null,
                Generator.MakeRandomLocation());
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceFormattedAddressShouldThrow(string formattedAddress)
        {
            new PlaceModel(
                Fakers.EnglishFaker.Random.Uuid().ToString(),
                formattedAddress,
                Generator.MakeRandomLocation());
        }
    }
}