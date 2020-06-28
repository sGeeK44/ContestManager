using System;
using Contest.Domain.Settings;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class AddressFactoryTest
    {
        [Test]
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