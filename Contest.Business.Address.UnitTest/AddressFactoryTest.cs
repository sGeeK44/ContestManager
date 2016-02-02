using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contest.Business.UnitTest
{
    [TestClass()]
    public class AddressFactoryTest
    {
        [TestMethod()]
        public void CreateTest()
        {
            var factory = new AddressFactory();
            var result = factory.Create(11, "street", "zipcode", "city");

            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(11, result.StreetNumber);
            Assert.AreEqual("street", result.Street);
            Assert.AreEqual("zipcode", result.ZipCode);
            Assert.AreEqual("city", result.City);
        }
    }
}