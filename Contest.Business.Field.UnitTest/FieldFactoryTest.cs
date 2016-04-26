using Contest.Core.Component;
using Contest.UnitTest.Kit;
using Moq;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class FieldFactoryTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var customComposer = new CustomComposer();
            customComposer.AddType(typeof(RepositoryContestMock));
            customComposer.AddType(typeof(RepositoryMatchMock));
            FlippingContainer.Instance.Current = customComposer;
        }

        [TestCase]
        public void Create_CorrectArg_ShouldReturnAField()
        {
            var factory = new FieldFactory();
            var result = factory.Create(new Mock<IContest>().Object, "name");

            Assert.IsInstanceOf<Field>(result);
        }
    }
}