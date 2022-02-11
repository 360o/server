using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfferItemModel = _360o.Server.Api.V1.Stores.Model.OfferItem;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.OfferItem
{
    [TestClass]
    public class OfferItemConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnOfferItem()
        {
            var itemId = Fakers.EnglishFaker.Random.Uuid();
            var quantity = Fakers.EnglishFaker.Random.Int(min: 1);

            var offerItem = new OfferItemModel(itemId, quantity);

            Assert.AreEqual(itemId, offerItem.ItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenEmptyItemIdShouldThrow()
        {
            var quantity = Fakers.EnglishFaker.Random.Int(min: 1);

            new OfferItemModel(Guid.Empty, quantity);
        }
    }
}
