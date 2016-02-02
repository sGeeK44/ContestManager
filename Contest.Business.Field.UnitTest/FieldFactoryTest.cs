using System;
using Contest.Core.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contest.Business.Fields.UnitTest
{
    [TestClass()]
    public class FieldFactoryTest
    {
        [TestInitialize]
        public void Init()
        {
            FlippingContainer.Instance.Current = new ExecutingAssemblies();
        }

        [TestMethod()]
        public void CreateTest()
        {
            var contest = new ContestMock();
            var factory = new FieldFactory();
            var result = factory.Create(contest, "name");

            Assert.IsInstanceOfType(result, typeof(Field));
        }
    }
}