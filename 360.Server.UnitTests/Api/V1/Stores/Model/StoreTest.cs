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

            var store = new Store(organizationId);

            Assert.AreEqual(organizationId, store.OrganizationId);
            Assert.IsNull(store.Place);
        }

        [TestMethod]
        public void SetPlace()
        {
            var store = MakeRandomStore();

            var place = MakeRandomPlace();

            store.SetPlace(place);

            Assert.AreEqual(place, store.Place);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'GooglePlaceId')")]
        public void GivenNullGooglePlaceIdSetPlaceShouldThrow()
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(null!, testPlace.FormattedAddress, testPlace.Location);

            store.SetPlace(place);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException), "Required input GooglePlaceId was empty. (Parameter 'GooglePlaceId')")]
        public void GivenWhitespaceGooglePlaceIdShouldThrow(string googlePlaceId)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(googlePlaceId, testPlace.FormattedAddress, testPlace.Location);

            store.SetPlace(place);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'FormattedAddress')")]
        public void GivenNullFormattedAddressShouldThrow()
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, null!, testPlace.Location);

            store.SetPlace(place);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [ExpectedException(typeof(ArgumentException), "Required input FormattedAddress was empty. (Parameter 'FormattedAddress')")]
        public void GivenWhitespaceFormattedAddressShouldThrow(string formattedAddress)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, formattedAddress, testPlace.Location);

            store.SetPlace(place);
        }

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Input Latitude was out of range (Parameter 'Latitude')")]
        public void GivenInvalidLatitudeShouldThrow(double latitude)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, testPlace.FormattedAddress, new Location
            {
                Latitude = latitude,
                Longitude = testPlace.Location.Longitude
            });

            store.SetPlace(place);
        }

        [DataTestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Input Longitude was out of range (Parameter 'Latitude')")]
        public void GivenInvalidLongitudeShouldThrow(double longitude)
        {
            var store = MakeRandomStore();
            var testPlace = MakeRandomPlace();
            var place = new Place(testPlace.GooglePlaceId, testPlace.FormattedAddress, new Location
            {
                Latitude = testPlace.Location.Longitude,
                Longitude = longitude
            });

            store.SetPlace(place);
        }

        private Store MakeRandomStore()
        {
            var organizationId = _faker.Random.Uuid();

            return new Store(organizationId);
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