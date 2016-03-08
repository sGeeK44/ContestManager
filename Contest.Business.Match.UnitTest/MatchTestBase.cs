using System;
using Contest.Core.Component;
using Moq;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    public class MatchTestBase
    {
        [TestFixtureSetUp]
        public void Init()
        {
            FlippingContainer.Instance.Current = new ExecutingAssemblies();
        }

        protected Mock<IGameStep> CreateGameStepStub1()
        {
            return Helper.CreateMock<IGameStep>("00000000-0000-0000-0000-000000000001");
        }

        protected Mock<ITeam> CreateTeamStub1()
        {
            return Helper.CreateMock<ITeam>("00000000-0000-0000-0001-000000000001");
        }

        protected Mock<ITeam> CreateTeamStub2()
        {
            return Helper.CreateMock<ITeam>("00000000-0000-0000-0001-000000000002");
        }

        protected Mock<ITeam> CreateTeamStub3()
        {
            return Helper.CreateMock<ITeam>("00000000-0000-0000-0001-000000000003");
        }

        protected Mock<IMatchSetting> CreateMatchSettingStub1()
        {
            return Helper.CreateMock<IMatchSetting>("00000000-0000-0000-0002-000000000001");
        }

        protected Mock<IField> CreateFieldStub1()
        {
            return Helper.CreateMock<IField>("00000000-0000-0000-0003-000000000001");
        }

        protected Mock<IMatch> CreateMatchStub1()
        {
            return Helper.CreateMock<IMatch>("00000000-0000-0000-0004-000000000001");
        }

        protected IMatch CreateMatch()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            return new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);
        }

        protected IMatch CreateStartedMatch()
        {
            var result = CreateMatch();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            result.Start(field.Object);
            return result;
        }
    }
}
