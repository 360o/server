using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OfferModel = _360o.Server.Api.V1.Stores.Model.Offer;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Offer
{
    [TestClass]
    public class OfferConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnOffer()
        {
            var storeId = Fakers.EnglishFaker.Random.Uuid();

            var offer = new OfferModel(storeId);

            Assert.AreEqual(offer.StoreId, storeId);
            Assert.AreEqual(Guid.Empty, offer.Id);
            Assert.IsNull(offer.EnglishName);
            Assert.IsNull(offer.FrenchName);
            Assert.IsFalse(offer.OfferItems.Any());
            Assert.IsNull(offer.Discount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenEmptyStoreIdShouldThrow()
        {
            new OfferModel(Guid.Empty);
        }
    }
}