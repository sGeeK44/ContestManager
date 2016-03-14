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

        protected Mock<IField> CreateFieldStub2()
        {
            return Helper.CreateMock<IField>("00000000-0000-0000-0003-000000000002");
        }

        protected Mock<IMatch> CreateMatchStub1()
        {
            return Helper.CreateMock<IMatch>("00000000-0000-0000-0004-000000000001");
        }

        protected IMatch CreatePlannedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null)
        {
            gameStep = gameStep ?? CreateGameStepStub1();
            team1 = team1?? CreateTeamStub1();
            team2 = team2 ?? CreateTeamStub2();
            matchSetting = matchSetting ?? CreateMatchSettingStub1();
            return new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);
        }

        protected IMatch CreateStartedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null, Mock<IField> field = null)
        {
            var result = CreatePlannedMatch(gameStep, team1, team2, matchSetting);
            field = field ?? CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            result.Start(field.Object);
            return result;
        }

        protected IMatch CreateFinishedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null, Mock<IField> field = null)
        {
            matchSetting = GetDefaultMatchSettingIfNull(matchSetting);
            var result = CreateStartedMatch(gameStep, team1, team2, matchSetting, field);
            result.SetResult(1, 0);
            return result;
        }

        protected IMatch CreateClosedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null, Mock<IField> field = null)
        {
            var result = CreateFinishedMatch(gameStep, team1, team2, matchSetting, field);
            result.Close();
            return result;
        }

        protected IMatch CreateFinishedMatch(ushort scoreTeam1, ushort scoreTeam2)
        {
            var matchSetting = GetDefaultMatchSetting();
            string mess;
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(scoreTeam1, scoreTeam2, out mess)).Returns(true);
            return CreateFinishedMatch(matchSetting: matchSetting);
        }

        protected Mock<IMatchSetting> GetDefaultMatchSetting()
        {
            string mess;
            var matchSetting = CreateMatchSettingStub1();
            matchSetting.Setup(_ => _.IsValidScore(1, out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidScore(0, out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(1, 0, out mess)).Returns(true);

            return matchSetting;
        }

        private Mock<IMatchSetting> GetDefaultMatchSettingIfNull(Mock<IMatchSetting> matchSetting)
        {
            if (matchSetting != null) return matchSetting;

            return GetDefaultMatchSetting();
        }
    }
}
