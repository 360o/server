using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite.Geometries;
using System;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Store
{
    [TestClass]
    public class PlaceTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var store = Generator.MakeRandomStore();

            var place = Generator.MakeRandomPlace();

            var expectedPoint = new Point(
                x: place.Location.Longitude,
                y: place.Location.Latitude);

            store.Place = place;

            Assert.AreEqual(place, store.Place);
            Assert.AreEqual(expectedPoint.X, store.Place.Location.Longitude);
            Assert.AreEqual(expectedPoint.Y, store.Place.Location.Latitude);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullPlaceShouldThrow()
        {
            var store = Generator.MakeRandomStore();

            store.Place = null;
        }
    }
}