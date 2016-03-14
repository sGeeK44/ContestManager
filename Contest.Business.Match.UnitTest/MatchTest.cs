using System;
using Contest.Core.Repository.Sql;
using Moq;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class MatchTest : MatchTestBase
    {
        #region TeamIsInvolve

        [TestCase]
        public void IsTeamInvolved_TeamIsFirstOpponentNotSameInstance_ShouldReturnFalse()
        {
            var match = CreatePlannedMatch();

            Assert.AreEqual(true, match.IsTeamInvolved(match.Team1));
        }

        [TestCase]
        public void IsTeamInvolved_TeamIsSecondOpponentNotSameInstance_ShouldReturnTrue()
        {
            var match = CreatePlannedMatch();

            Assert.AreEqual(true, match.IsTeamInvolved(match.Team2));
        }

        [TestCase]
        public void IsTeamInvolved_TeamIsFirstOpponent_ShouldReturnFalse()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var match = new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);
            
            Assert.AreEqual(true, match.IsTeamInvolved(team1.Object));
        }

        [TestCase]
        public void IsTeamInvolved_TeamIsSecondOpponent_ShouldReturnTrue()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var match = new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);

            Assert.AreEqual(true, match.IsTeamInvolved(team2.Object));
        }

        [TestCase]
        public void IsTeamInvolved_TeamIsNotAnOpponent_ShouldReturnFalse()
        {
            var team3 = CreateTeamStub3();
            var match = CreatePlannedMatch();
            
            Assert.AreEqual(false, match.IsTeamInvolved(team3.Object));
        }

        #endregion

        #region Planned match

        [TestCase]
        public void Start_PlannedMatchWithNoField_ShouldThrowException()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var match = new MatchFactory().Create(Helper.GetMock(gameStep), Helper.GetMock(team1), Helper.GetMock(team2), Helper.GetMock(matchSetting));
            
            Assert.Throws<ArgumentNullException>(() => match.Start(null));
        }

        [TestCase]
        public void Start_PlannedMatchFieldAlreadyBusy_ShouldThrowArgumentException()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var field = CreateFieldStub1();
            var match = new MatchFactory().Create(Helper.GetMock(gameStep), Helper.GetMock(team1), Helper.GetMock(team2), Helper.GetMock(matchSetting));

            field.SetupGet(_ => _.IsAllocated).Returns(true);
            field.SetupGet(_ => _.MatchInProgess).Returns(Helper.CreateMock<IMatch>("00000000-0000-0000-0000-000000000010").Object);

            Assert.Throws<ArgumentException>(() => match.Start(field.Object));
        }
        
        [TestCase]
        public void Start_PlannedMatchTeam1AlreadyInGame()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var matchTeam1 = CreateMatchStub1();
            team1.SetupGet(_ => _.CurrentMatch).Returns(matchTeam1.Object);
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            var match = new MatchFactory().Create(Helper.GetMock(gameStep), Helper.GetMock(team1), Helper.GetMock(team2), Helper.GetMock(matchSetting));
            
            Assert.Throws<NotSupportedException>(() => match.Start(field.Object));
        }

        [TestCase]
        public void Start_PlannedMatchTeam2AlreadyInGame()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchTeam2 = CreateMatchStub1();
            team2.SetupGet(_ => _.CurrentMatch).Returns(matchTeam2.Object);
            var matchSetting = CreateMatchSettingStub1();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            var match = new MatchFactory().Create(Helper.GetMock(gameStep), Helper.GetMock(team1), Helper.GetMock(team2), Helper.GetMock(matchSetting));

            Assert.Throws<NotSupportedException>(() => match.Start(field.Object));
        }
        
        [TestCase]
        public void Start_PlannedMatchAllFine_StartEventShouldRaised()
        {
            var match = CreatePlannedMatch();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            var startEventIsRaised = false;
            match.MatchStarted += sender => startEventIsRaised = true;

            match.Start(field.Object);

            Assert.AreEqual(true, startEventIsRaised);
        }

        [TestCase]
        public void Start_PlannedMatchAllFine_MatchShouldBeInitialized()
        {
            var match = CreatePlannedMatch();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);

            match.Start(field.Object);
            
            Assert.AreEqual(field.Object, match.MatchField);
            Assert.AreEqual(0, match.TeamScore1);
            Assert.AreEqual(0, match.TeamScore2);
        }

        [TestCase]
        public void GetBeginningAt_StartedMatch_ShouldReturnNotNull()
        {
            var match = CreateStartedMatch();
            
            Assert.AreNotEqual(null, match.Beginning);
        }

        [TestCase]
        public void GetEnddedAt_StartedMatch_ShouldReturnNull()
        {
            var match = CreateStartedMatch();
            
            Assert.AreEqual(null, match.Endded);
        }

        [TestCase]
        public void GetIsBeginning_StartedMatch_ShouldReturnTrue()
        {
            var match = CreateStartedMatch();

            Assert.AreEqual(true, match.IsBeginning);
        }

        [TestCase]
        public void GetIsClose_StartedMatch_ShouldReturnFalse()
        {
            var match = CreateStartedMatch();

            Assert.AreEqual(false, match.IsClose);
        }

        [TestCase]
        public void GetIsFinish_StartedMatch_ShouldReturnFalse()
        {
            var match = CreateStartedMatch();
            
            Assert.AreEqual(false, match.IsFinished);
        }

        [TestCase]
        public void GetMatchState_StartedMatch_ShouldReturnInProgress()
        {
            var match = CreateStartedMatch();
            
            Assert.AreEqual(MatchState.InProgress, match.MatchState);
        }

        [TestCase]
        public void GetWinner_StartedMatch_ShouldReturnNull()
        {
            var match = CreateStartedMatch();
            
            Assert.AreEqual(null, match.Winner);
        }

        [TestCase]
        public void Close_PlannedMatch_ShouldThrowException()
        {
            var match = CreatePlannedMatch();
            
            Assert.Throws<NotSupportedException>(() => match.Close());
        }

        [TestCase]
        public void SetResult_PlannedMatchShouldThrowException()
        {
            var match = CreatePlannedMatch();

            Assert.Throws<NotSupportedException>(() => match.SetResult(0, 0));
        }

        [TestCase]
        public void UpdateScore_PlannedMatch_ShouldThrowException()
        {
            var match = CreatePlannedMatch();

            Assert.Throws<NotSupportedException>(() => match.UpdateScore(0, 0));
        }

        #endregion

        #region InProgress match

        [TestCase]
        public void Start_InProgressMatch_ShouldThrowException()
        {
            var field = CreateFieldStub2();
            var match = CreateStartedMatch();
            
            Assert.Throws<NotSupportedException>(() => match.Start(field.Object));
        }
        
        [TestCase]
        public void UpdateScore_InProgressMatchUnvalidTeam1Result_ShouldThrowException()
        {
            string mess;
            ushort ts1 = 14;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(ts1, out mess)).Returns(false);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            Assert.Throws<ArgumentException>(() => match.UpdateScore(ts1, 0));
        }
        
        [TestCase]
        public void UpdateScore_InProgressMatchUnvalidTeam2Result_ShouldThrowException()
        {
            string mess;
            ushort ts1 = 5;
            ushort ts2 = 14;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(ts1, out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidScore(ts2, out mess)).Returns(false);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            Assert.Throws<ArgumentException>(() => match.UpdateScore(ts1, ts2));
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidTeamResult_UpdateEventShouldBeRaiseOnce()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;
            
            match.UpdateScore(5, 0);
            
            Assert.AreEqual(1, updateEventIsRaised);
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidTeam1Result_Team1ScoreShouldBeEqualToSpecifiedValid()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.UpdateScore(5, 0);
            
            Assert.AreEqual(5, match.TeamScore1);
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidTeam2Result_Team2ScoreShouldBeEqualToSpecifiedValid()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.UpdateScore(0, 5);

            Assert.AreEqual(5, match.TeamScore2);
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidResult_MatchShouldBeNotNull()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.UpdateScore(0, 5);

            Assert.IsNotNull(match.MatchField);
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidResult_EndAtShouldBeNull()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.UpdateScore(0, 5);

            Assert.IsNull(match.Endded);
        }

        [TestCase]
        public void UpdateScore_InProgressMatchValidResult_StateShouldBeEqualInProgress()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.UpdateScore(0, 5);

            Assert.AreEqual(MatchState.InProgress, match.MatchState);
        }
        
        [TestCase]
        public void SetResult_InProgressMatchUnvalidTeamResult_ShouldThrowException()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(false);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            Assert.Throws<ArgumentException>(() => match.SetResult(14, 14));
        }

        [TestCase]
        public void SetResult_InProgressMatchValidTeamResult_UpdateEventShouldBeRaiseOnce()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;

            match.SetResult(5, 0);
            
            Assert.AreEqual(1, updateEventIsRaised);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidTeamResult_EndEventShouldBeRaiseOnce()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);
            var endEventIsRaised = 0;
            match.MatchEnded += sender => endEventIsRaised++;

            match.SetResult(5, 0);
            
            Assert.AreEqual(1, endEventIsRaised);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidTeam1Result_Team1ScoreShouldBeEqualToSpecifiedValid()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(5, 0);

            Assert.AreEqual(5, match.TeamScore1);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidTeam2Result_Team2ScoreShouldBeEqualToSpecifiedValid()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);

            Assert.AreEqual(5, match.TeamScore2);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_MatchShouldBeNull()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);

            Assert.IsNull(match.MatchField);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_EndAtShouldBeNull()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);

            Assert.IsNotNull(match.Endded);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_StateShouldBeEqualInProgress()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);

            Assert.AreEqual(MatchState.Finished, match.MatchState);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_IsBeginningShouldReturnTrue()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);
            
            Assert.AreEqual(true, match.IsBeginning);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_IsFinishShouldReturnTrue()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);
            
            Assert.AreEqual(true, match.IsFinished);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResult_IsCloseShouldReturnFalse()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);
        
            Assert.AreEqual(false, match.IsClose);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResultTeam1Win_WinnerShouldReturnTeam1()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(5, 0);

            Assert.AreEqual(match.Team1, match.Winner);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResultTeam2Win_WinnerShouldReturnTeam2()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(0, 5);

            Assert.AreEqual(match.Team2, match.Winner);
        }

        [TestCase]
        public void SetResult_InProgressMatchValidResultDUce_WinnerShouldReturnNull()
        {
            string mess;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            var match = CreateStartedMatch(matchSetting: matchSetting);

            match.SetResult(5, 5);

            Assert.IsNull(match.Winner);
        }

        [TestCase]
        public void Close_InProgressMatch_ShouldThrowException()
        {
            var match = CreateStartedMatch();
            
            Assert.Throws<NotSupportedException>(() => match.Close());
        }

        #endregion

        #region Finished match

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void Start_FinishedMatch_ShouldThrowException()
        {
            var field = new Mock<IField>();
            var match = CreateFinishedMatch();
            
            match.Start(field.Object);
        }

        [TestCase]
        public void UpdateScore_FinishedScoreIsNotValid_ShouldThrowException()
        {
            var matchSetting = GetDefaultMatchSetting();
            string mess;
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(14, 0, out mess)).Returns(false);
            var match = CreateFinishedMatch(matchSetting : matchSetting);
            
            Assert.Throws<ArgumentException>(() => match.UpdateScore(14, 0));
        }

        [TestCase]
        public void UpdateScore_FinishedChangeWinner_WinnerShouldBeChange()
        {
            var match = CreateFinishedMatch(5, 14);

            match.UpdateScore(5, 14);

            Assert.AreEqual(match.Team2, match.Winner);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_TeamScore1ShouldBeUpdated()
        {
            var match = CreateFinishedMatch(14, 5);
            
            match.UpdateScore(14, 5);

            Assert.AreEqual(14, match.TeamScore1);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_TeamScore2ShouldBeUpdated()
        {
            var match = CreateFinishedMatch(14, 5);

            match.UpdateScore(14, 5);

            Assert.AreEqual(5, match.TeamScore2);
        }

        [TestCase]
        public void UpdateScore_FinishedSameValidScore_UpdateEventShouldNotBeRaise()
        {
            var match = CreateFinishedMatch();
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;

            match.UpdateScore(1, 0);

            Assert.AreEqual(0, updateEventIsRaised);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_UpdateEventShouldBeRaise()
        {
            var match = CreateFinishedMatch(14, 5);
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;

            match.UpdateScore(14, 5);

            Assert.AreEqual(1, updateEventIsRaised);
        }

        [TestCase]
        public void UpdateScore_FinishedTeam1WinValidScore_WinnerShouldBeTeam1()
        {
            var match = CreateFinishedMatch(14, 5);

            match.UpdateScore(14, 5);

            Assert.AreEqual(match.Team1, match.Winner);
        }

        [TestCase]
        public void UpdateScore_FinishedTeam2WinValidScore_WinnerShouldBeTeam2()
        {
            var match = CreateFinishedMatch(5, 14);

            match.UpdateScore(5, 14);

            Assert.AreEqual(match.Team2, match.Winner);
        }

        [TestCase]
        public void UpdateScore_FinishedDuceValidScore_WinnerShouldBeNull()
        {
            var match = CreateFinishedMatch(14, 14);

            match.UpdateScore(14, 14);

            Assert.IsNull(match.Winner);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_MatchFIeldShouldBeNull()
        {
            var match = CreateFinishedMatch(13, 5);

            match.UpdateScore(13, 5);

            Assert.IsNull(match.MatchField);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_EndDateAtShouldNotNull()
        {
            var match = CreateFinishedMatch(13, 5);

            match.UpdateScore(13, 5);

            Assert.IsNotNull(match.Endded);
        }

        [TestCase]
        public void UpdateScore_FinishedValidScore_MatchStateShouldEqualFinished()
        {
            var match = CreateFinishedMatch(13, 5);

            match.UpdateScore(13, 5);

            Assert.AreEqual(MatchState.Finished, match.MatchState);
        }

        [TestCase]
        public void SetScore_FinishedSetValidScoreTwice_ShouldThrowException()
        {
            var match = CreateFinishedMatch();
            Assert.Throws<NotSupportedException>(() => match.SetResult(13, 5));
        }

        [TestCase]
        public void Close_FinishedMatch_EndedAtShouldBeNotNull()
        {
            var match = CreateFinishedMatch();
            
            match.Close();
            
            Assert.IsNotNull(match.Endded);
        }

        [TestCase]
        public void Close_FinishedMatch_StateShouldEqualInternal()
        {
            var match = CreateFinishedMatch();

            match.Close();
            
            Assert.AreEqual(MatchState.Closed, match.MatchState);
        }

        [TestCase]
        public void Close_FinishedMatch_IsCloseShouldReturnTrue()
        {
            var match = CreateFinishedMatch();

            match.Close();

            Assert.IsTrue(match.IsClose);
        }

        [TestCase]
        public void Close_FinishedMatch_IsBeginningShouldReturnTrue()
        {
            var match = CreateFinishedMatch();

            match.Close();

            Assert.AreEqual(true, match.IsBeginning);
        }

        [TestCase]
        public void Close_FinishedMatch_IsFinishedShouldBeTrue()
        {
            var match = CreateFinishedMatch();

            match.Close();

            Assert.AreEqual(true, match.IsFinished);
        }

        #endregion

        #region Closed match

        [TestCase]
        public void Start_ClosedMatch_ShouldThrowException()
        {
            var match = CreateClosedMatch();
            var field = new Mock<IField>().Object;

            Assert.Throws<NotSupportedException>(() => match.Start(field));
        }

        [TestCase]
        public void UpdateScore_ClosedMatch_ShouldThrowException()
        {
            var match = CreateClosedMatch();

            Assert.Throws<NotSupportedException>(() => match.UpdateScore(0, 0));
        }

        [TestCase]
        public void SetResult_ClosedMatch_ShouldThrowException()
        {
            var match = CreateClosedMatch();

            Assert.Throws<NotSupportedException>(() => match.SetResult(0, 0));
        }

        [TestCase]
        public void Close_ClosedMatch_ShouldThrowException()
        {
            var match = CreateClosedMatch();

            Assert.Throws<NotSupportedException>(() => match.Close());
        }

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var match = new Match();

            Assert.Throws<ArgumentNullException>(() => match.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var match = new Match();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IMatch>(match)).Verifiable();

            match.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var match = new Match();

            Assert.Throws<ArgumentNullException>(() => match.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var match = new Match();

            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IMatch>(match)).Verifiable();

            match.PrepareDelete(repoMock.Object);
        }

        #endregion
        
        [TestCase]
        public void GetTimeElapse_PlannedMatch_ShouldBeNull()
        {
            var match = CreatePlannedMatch();

            Assert.IsNull(match.Elapse);
        }

        [TestCase]
        public void GetTimeElapse_InProgressMatch_ShouldBeNotNull()
        {
            var match = CreateStartedMatch();

            Assert.IsNotNull(match.Elapse);
        }

        [TestCase]
        public void GetTimeElapse_FinishedMatch_ShouldBeNotNull()
        {
            var match = CreateFinishedMatch();

            Assert.IsNotNull(match.Elapse);
        }
    }
}
