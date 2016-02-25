using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Contest.Business.Fields.UnitTest
{
    [TestClass()]
    public class FieldFactoryTest
    {
        [TestMethod()]
        public void Create_CorrectArg_ShouldReturnAField()
        {
            var factory = new FieldFactory();
            var result = factory.Create(new Mock<IContest>().Object, "name");

            Assert.IsInstanceOfType(result, typeof(Field));
        }
    }
}