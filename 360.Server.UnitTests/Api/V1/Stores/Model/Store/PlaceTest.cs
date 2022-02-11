using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            store.Place = place;

            Assert.AreEqual(place, store.Place);
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