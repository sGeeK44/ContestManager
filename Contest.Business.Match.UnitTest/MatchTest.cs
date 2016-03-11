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
            var match = CreateMatch();

            Assert.AreEqual(true, match.IsTeamInvolved(match.Team1));
        }

        [TestCase]
        public void IsTeamInvolved_TeamIsSecondOpponentNotSameInstance_ShouldReturnTrue()
        {
            var match = CreateMatch();

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
            var match = CreateMatch();
            
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
            var match = CreateMatch();
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
            var match = CreateMatch();
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
            var match = CreateMatch();
            
            Assert.Throws<NotSupportedException>(() => match.Close());
        }

        [TestCase]
        public void SetResult_PlannedMatchShouldThrowException()
        {
            var match = CreateMatch();

            Assert.Throws<NotSupportedException>(() => match.SetResult(0, 0));
        }

        [TestCase]
        public void UpdateScore_PlannedMatch_ShouldThrowException()
        {
            var match = CreateMatch();

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

        [TestCase(14, 0)]
        [TestCase(14, 1)]
        public void UpdateScore_InProgressMatchUnvalidTeamResult_ShouldThrowException(int teamScore1, int teamScore2)
        {
            string mess;
            ushort ts1 = (ushort)teamScore1;
            ushort ts2 = (ushort)teamScore2;
            var matchSetting = new Mock<IMatchSetting>();
            matchSetting.Setup(_ => _.IsValidScore(ts1, out mess)).Returns(false);
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

        // Update score team 1 overflow
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 14, false, 0, true, false, 0, 0, ExpectedException = typeof(ArgumentException))]
        // Update score team 2 overflow
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 0, true, 14, false, false, 0, 0, ExpectedException = typeof(ArgumentException))]
        // Update score with no match point
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 0, true, 0, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        // Update score with no match point
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 5, true, 5, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        // Try to put duce when no duce is setted
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 13, true, 13, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        // Duce match
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 13, true, 13, true, true, 0, 1)]
        // No update
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 13, true, 0, true, true, 1, 0)]
        // Change winner
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 0, true, 13, true, true, 2, 1)]
        // No update
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 13, 13, true, 13, true, true, 0, 0)]
        // Change winner
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 13, 13, true, 0, true, true, 1, 1)]
        // Change winner
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 0, true, 13, true, true, 2, 1)]
        public void UpdateScoreFinishedMatchTest(string gameStepId, string team1Id, string team2Id, string matchSettingId, string fieldId,
                                                 int initialTeamScore1, int initialTeamScore2, int teamScore1, bool isValidScore1, int teamScore2, bool isValidScore2, bool isValidFinishedScore, int exceptedWinner, int countRaiseEvent)
        {
            // Arrange
            var matchSetting = Helper.CreateMock<IMatchSetting>(matchSettingId);
            string mess;
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(isValidScore1);
            matchSetting.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(isValidScore2);
            string message;
            matchSetting.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out message)).Returns(isValidFinishedScore);
            var match = Helper.CreateMatch(gameStepId, team1Id, team2Id, matchSetting.Object);
            var field = Helper.CreateMock<IField>(fieldId);
            match.Start(field.Object);
            match.SetResult((ushort)initialTeamScore1, (ushort)initialTeamScore2);
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;

            //Act
            match.UpdateScore((ushort)teamScore1, (ushort)teamScore2);

            //Assert
            Assert.AreEqual(countRaiseEvent, updateEventIsRaised);
            Assert.IsTrue(match.TeamScore1 == teamScore1);
            Assert.IsTrue(match.TeamScore2 == teamScore2);
            switch (exceptedWinner)
            {
                case 1:
                    Assert.AreEqual(match.Team1, match.Winner);
                    break;
                case 2:
                    Assert.AreEqual(match.Team2, match.Winner);
                    break;
                default:
                    Assert.IsNull(match.Winner);
                    break;
            }
            Assert.IsNull(match.MatchField);
            Assert.IsNotNull(match.Endded);
            Assert.AreEqual(MatchState.Finished, match.MatchState);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void SetResultFinishedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);

            //Act
            match.SetResult(1, 0);
        }

        [TestCase]
        public void CloseFinishedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);

            //Act
            match.Close();

            // Assert
            Assert.IsNotNull(match.Endded);
            Assert.AreEqual(MatchState.Closed, match.MatchState);
            Assert.AreEqual(true, match.IsClose);
            Assert.AreEqual(true, match.IsBeginning);
            Assert.AreEqual(true, match.IsFinished);
        }

        #endregion

        #region Closed match

        [TestCase(1, 1, 0, true)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 1, 1, null)]
        public void WinnerClosedMatchTest(int matchPoint, int score1, int score2, bool? isWinner1)
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, (ushort)matchPoint);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult((ushort)score1, (ushort)score2);
            match.Close();

            // Assert
            if (isWinner1 == null) Assert.IsNull(match.Winner);
            else if (isWinner1.Value) Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000002"), match.Winner.Id);
            else Assert.AreEqual(new Guid("00000000-0000-0000-0000-000000000003"), match.Winner.Id);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void StartClosedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);
            match.Close();

            // Act and assert
            match.Start(field.Object);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void UpdateScoreClosedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);
            match.Close();

            // Act and assert
            match.UpdateScore(0, 0);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void SetResultClosedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);
            match.Close();

            // Act and assert
            match.SetResult(0, 0);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void CloseClosedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);
            match.SetResult(0, 1);
            match.Close();

            // Act and assert
            match.Close();
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
    }
}
