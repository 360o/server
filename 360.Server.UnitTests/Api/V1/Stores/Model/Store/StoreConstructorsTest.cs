using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite.Geometries;
using System;
using StoreModel = _360o.Server.Api.V1.Stores.Model.Store;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Store
{
    [TestClass]
    public class StoreConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnStore()
        {
            var organizationId = Fakers.EnglishFaker.Random.Uuid();
            
            var place = Generator.MakeRandomPlace();

            var expectedPoint = new Point(
                x: place.Location.Longitude,
                y: place.Location.Latitude);

            var store = new StoreModel(organizationId, place);

            Assert.AreEqual(organizationId, store.OrganizationId);
            Assert.AreEqual(place, store.Place);
            Assert.AreEqual(expectedPoint.X, store.Place.Location.Longitude);
            Assert.AreEqual(expectedPoint.Y, store.Place.Location.Latitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenEmptyOrganizationIdShouldThrow()
        {
            var place = Generator.MakeRandomPlace();

            new StoreModel(Guid.Empty, place);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullPlaceShouldThrow()
        {
            var organizationId = Fakers.EnglishFaker.Random.Uuid();

            new StoreModel(organizationId, null);
        }
    }
}