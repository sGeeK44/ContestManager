using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Contest.Business.UnitTest
{
    [TestClass()]
    public class GameSettingTest
    {
        [TestMethod()]
        public void Equal_SameContentValue_ShouldBeEqual()
        {
            var factory = new GameSettingFactory();
            var obj1 = factory.Create(1, 1);
            var obj2 = factory.Create(1, 1);

            Assert.IsTrue(obj1.Equals(obj2));
            Assert.IsTrue(obj2.Equals(obj1));
        }

        [TestMethod()]
        public void Equal_DifferentContentValue_ShouldNotBeEqual()
        {
            var factory = new GameSettingFactory();
            var obj1 = factory.Create(1, 3);
            var obj2 = factory.Create(1, 2);

            Assert.IsFalse(obj1.Equals(obj2));
            Assert.IsFalse(obj2.Equals(obj1));
        }

        [TestMethod()]
        public void Equal_NullArg_ShouldNotBeEqual()
        {
            var factory = new GameSettingFactory();
            var obj1 = factory.Create(5, 10);

            Assert.IsFalse(obj1.Equals(null));
        }

        [TestMethod()]
        public void Equal_ArgDoesNotImplementIGameSetting_ShouldNotBeEqual()
        {
            var factory = new GameSettingFactory();
            var obj1 = factory.Create(5, 10);

            Assert.IsFalse(obj1.Equals(new object()));
        }
    }
}