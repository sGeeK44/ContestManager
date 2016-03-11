﻿using System;
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

        protected IMatch CreateMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null)
        {
            gameStep = gameStep ?? CreateGameStepStub1();
            team1 = team1?? CreateTeamStub1();
            team2 = team2 ?? CreateTeamStub2();
            matchSetting = matchSetting ?? CreateMatchSettingStub1();
            return new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);
        }

        protected IMatch CreateStartedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null, Mock<IField> field = null)
        {
            var result = CreateMatch(gameStep, team1, team2, matchSetting);
            field = field ?? CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            result.Start(field.Object);
            return result;
        }

        protected IMatch CreateFinishedMatch(Mock<IGameStep> gameStep = null, Mock<ITeam> team1 = null, Mock<ITeam> team2 = null, Mock<IMatchSetting> matchSetting = null, Mock<IField> field = null)
        {
            string mess;
            matchSetting = matchSetting ?? new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var result = CreateStartedMatch(matchSetting: matchSetting);
            result.SetResult(1, 0);
            return result;
        }
    }
}
