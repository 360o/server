using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LocationModel = _360o.Server.Api.V1.Stores.Model.Location;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Location
{
    [TestClass]
    public class LocationConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnLocation()
        {
            var latitude = Fakers.EnglishFaker.Address.Latitude();
            var longitude = Fakers.EnglishFaker.Address.Longitude();

            var location = new LocationModel(latitude, longitude);

            Assert.AreEqual(latitude, location.Latitude);
            Assert.AreEqual(longitude, location.Longitude);
        }

        [DataTestMethod]
        [DataRow(-91)]
        [DataRow(91)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenInvalidLatitudeShouldThrow(double latitude)
        {
            var longitude = Fakers.EnglishFaker.Address.Longitude();

            new LocationModel(latitude, longitude);
        }

        [DataTestMethod]
        [DataRow(-181)]
        [DataRow(181)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GivenInvalidLongitudeShouldThrow(double longitude)
        {
            var latitude = Fakers.EnglishFaker.Address.Latitude();

            new LocationModel(latitude, longitude);
        }
    }
}