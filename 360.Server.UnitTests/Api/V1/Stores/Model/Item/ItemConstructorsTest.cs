using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ItemModel = _360o.Server.Api.V1.Stores.Model.Item;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class ItemConstructorsTest
    {
        [TestMethod]
        public void GivenValidArgumentsShouldReturnItem()
        {
            var storeId = Fakers.EnglishFaker.Random.Uuid();

            var item = new ItemModel(storeId);

            Assert.AreEqual(storeId, item.StoreId);
            Assert.IsNull(item.EnglishName);
            Assert.IsNull(item.EnglishDescription);
            Assert.IsNull(item.FrenchName);
            Assert.IsNull(item.FrenchDescription);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenEmptyStoreIdShouldThrow()
        {
            new ItemModel(Guid.Empty);
        }
    }
}