using _360o.Server.Api.V1.Stores.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyValueModel = _360o.Server.Api.V1.Stores.Model.MoneyValue;

namespace _360.Server.UnitTests.Api.V1.Stores.Model.Item
{
    [TestClass]
    public class PriceTest
    {
        [TestMethod]
        public void GivenRandomArgumentShouldSetToArgument()
        {
            var item = Generator.MakeRandomItem();

            var price = Generator.MakeRandomMoneyValue();

            item.Price = price;

            Assert.AreEqual(price, item.Price);
        }

        [TestMethod]
        public void GivenNullShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.Price = null;

            Assert.IsNull(item.Price);
        }

        [TestMethod]
        public void GivenZeroShouldSetToNull()
        {
            var item = Generator.MakeRandomItem();

            item.Price = new MoneyValueModel(
                0,
                Fakers.EnglishFaker.PickRandom<Iso4217CurrencyCode>()
                );

            Assert.IsNull(item.Price);
        }
    }
}