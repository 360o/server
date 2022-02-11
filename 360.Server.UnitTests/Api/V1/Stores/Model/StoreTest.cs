using _360o.Server.Api.V1.Stores.Model;
using Bogus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _360.Server.UnitTests.Api.V1.Stores.Model
{
    [TestClass]
    public class StoreTest
    {
        private readonly Faker _faker = new Faker();

        [TestMethod]
        public void GivenValidArgumentsShouldReturnStore()
        {
            var organizationId = _faker.Random.Uuid();
            var place = MakeRandomPlace();

            var store = new Store(organizationId, place);

            Assert.AreEqual(organizationId, store.OrganizationId);
            Assert.AreEqual(place, store.Place);
        }

        [TestMethod]
        public void SetPlace()
        {
            var store = MakeRandomStore();

            var place = MakeRandomPlace();

            store.Place = place;

            Assert.AreEqual(place, store.Place);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullGooglePlaceIdSetPlaceShouldThrow()
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(null!, testPlace.FormattedAddress, testPlace.Location);

            store.Place = place;
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceGooglePlaceIdShouldThrow(string googlePlaceId)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(googlePlaceId, testPlace.FormattedAddress, testPlace.Location);

            store.Place = place;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullFormattedAddressShouldThrow()
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, null!, testPlace.Location);

            store.Place = place;
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenWhitespaceFormattedAddressShouldThrow(string formattedAddress)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, formattedAddress, testPlace.Location);

            store.Place = place;
        }

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenInvalidLatitudeShouldThrow(double latitude)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, testPlace.FormattedAddress, new Location
            {
                Latitude = latitude,
                Longitude = testPlace.Location.Longitude
            });

            store.Place = place;
        }

        [DataTestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenInvalidLongitudeShouldThrow(double longitude)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, testPlace.FormattedAddress, new Location
            {
                Latitude = testPlace.Location.Longitude,
                Longitude = longitude
            });

            store.Place = place;
        }

        private Store MakeRandomStore()
        {
            var organizationId = _faker.Random.Uuid();
            var place = MakeRandomPlace();

            return new Store(organizationId, place);
        }

        private Place MakeRandomPlace()
        {
            return new Place(
                _faker.Random.String(),
                _faker.Address.FullAddress(),
                new Location
                {
                    Latitude = _faker.Address.Latitude(),
                    Longitude = _faker.Address.Longitude(),
                });
        }
    }
}