using System.ComponentModel.Composition;
using Contest.Business;
using Moq;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IFieldFactory))]
    public class FieldFactoryMock : IFieldFactory
    {
        public FieldFactoryMock()
        {
            Mock = new Mock<IFieldFactory>();
        }

        public FieldFactoryMock(Mock<IFieldFactory> mock)
        {
            Mock = mock;
        }

        public Mock<IFieldFactory> Mock { get; set; }

        public IField Create(IContest current, string name)
        {
            return Mock.Object.Create(current, name);
        }
    }
}