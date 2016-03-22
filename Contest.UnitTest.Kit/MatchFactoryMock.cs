using System.ComponentModel.Composition;
using Contest.Business;
using Moq;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IMatchFactory))]
    public class MatchFactoryMock : IMatchFactory
    {
        public MatchFactoryMock()
        {
            Mock = new Mock<IMatchFactory>();
        }

        public MatchFactoryMock(Mock<IMatchFactory> mock)
        {
            Mock = mock;
        }

        public Mock<IMatchFactory> Mock { get; set; }

        public IMatch Create(IGameStep gameStep, ITeam team1, ITeam team2, IMatchSetting setting)
        {
            return Mock.Object.Create(gameStep, team1, team2, setting);
        }
    }
}