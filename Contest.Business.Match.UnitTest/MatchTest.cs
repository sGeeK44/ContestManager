using System;
using Contest.Core.Component;
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
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();
            var field = CreateFieldStub1();
            field.SetupGet(_ => _.IsAllocated).Returns(false);
            var match = new MatchFactory().Create(Helper.GetMock(gameStep), Helper.GetMock(team1), Helper.GetMock(team2), Helper.GetMock(matchSetting));
            var startEventIsRaised = false;
            match.MatchStarted += sender => startEventIsRaised = true;

            match.Start(field != null ? field.Object : null);

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
        public void GetIsFInish_StartedMatch_ShouldReturnFalse()
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

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void ClosePlannedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004");

            // Act and assert
            match.Close();
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void SetResultPlannedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004");

            // Act and assert
            match.SetResult(0, 0);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void UpdateScorePlannedMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004");

            // Act and assert
            match.UpdateScore(0, 0);
        }

        #endregion

        #region InProgress match

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void StartInProgressMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);

            // Act and assert
            match.Start(field.Object);
        }

        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 14, false, 0, true, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, 14, false, true, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, true, 0, true, 0)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 5, true, 5, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 13, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 13, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 0, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, true, 13, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 13, 13, true, 13, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 13, true, 0, true, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 13, 0, true, 13, true, 1)]
        public void UpdateScoreStartedMatchTest(string gameStepId, string team1Id, string team2Id, string matchSettingId, string fieldId,
                                                int initialTeamScore1, int initialTeamScore2, int teamScore1, bool isValidScore1, int teamScore2, bool isValidScore2, int countRaiseEvent)
        {
            // Arrange
            var matchSetting = Helper.CreateMock<IMatchSetting>(matchSettingId);
            string mess;
            matchSetting.Setup(_ => _.IsValidScore((ushort)teamScore1, out mess)).Returns(isValidScore1);
            matchSetting.Setup(_ => _.IsValidScore((ushort)teamScore2, out mess)).Returns(isValidScore2);
            var match = Helper.CreateMatch(gameStepId, team1Id, team2Id, matchSetting.Object);
            var field = Helper.CreateMock<IField>(fieldId);
            match.Start(field.Object);
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;

            //Act
            match.UpdateScore((ushort)initialTeamScore1, (ushort)initialTeamScore2);
            match.UpdateScore((ushort)teamScore1, (ushort)teamScore2);

            //Assert
            Assert.AreEqual(countRaiseEvent, updateEventIsRaised);
            Assert.IsTrue(match.TeamScore1 == teamScore1);
            Assert.IsTrue(match.TeamScore2 == teamScore2);
            Assert.IsNotNull(match.MatchField);
            Assert.IsNull(match.Endded);
            Assert.AreEqual(MatchState.InProgress, match.MatchState);
        }

        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 14, false, 0, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, true, 14, false, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, true, 0, true, false, 0, 0, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 5, true, 5, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 13, true, false, 0, 1, ExpectedException = typeof(ArgumentException))]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 13, true, true, 0, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 13, true, 0, true, true, 1, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 0, 0, true, 13, true, true, 2, 1)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 13, 13, true, 13, true, true, 0, 0)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 13, 0, 13, true, 0, true, true, 1, 0)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000005", 0, 13, 0, true, 13, true, true, 2, 0)]
        public void SetResultInProgressMatchTest(string gameStepId, string team1Id, string team2Id, string matchSettingId, string fieldId,
                                                 int initialTeamScore1, int initialTeamScore2, int teamScore1, bool isValidScore1, int teamScore2, bool isValidScore2, bool isValidFinishedScore, int exceptedWinner, int exceptedScoreUpdateRaised)
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
            match.UpdateScore((ushort)initialTeamScore1, (ushort)initialTeamScore2);
            var updateEventIsRaised = 0;
            match.ScoreChanged += sender => updateEventIsRaised++;
            var endEventIsRaised = 0;
            match.MatchEnded += sender => endEventIsRaised++;

            //Act
            match.SetResult((ushort)teamScore1, (ushort)teamScore2);

            //Assert
            Assert.AreEqual(exceptedScoreUpdateRaised, updateEventIsRaised);
            Assert.AreEqual(1, endEventIsRaised);
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
            Assert.AreEqual(false, match.IsClose);
            Assert.AreEqual(true, match.IsBeginning);
            Assert.AreEqual(true, match.IsFinished);
        }

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void CloseInProgressMatchTest()
        {
            // Arrange
            var match = Helper.CreateMatch("00000000-0000-0000-0000-000000000001",
                                              "00000000-0000-0000-0000-000000000002",
                                              "00000000-0000-0000-0000-000000000003",
                                              "00000000-0000-0000-0000-000000000004",
                                              true, EndTypeConstaint.Point, null, 1);
            var field = Helper.CreateMock<IField>("00000000-0000-0000-0000-000000000005");
            match.Start(field.Object);

            // Act and assert
            match.Close();
        }

        #endregion

        #region Finished match

        [TestCase(ExpectedException = typeof(NotSupportedException))]
        public void StartFinishedMatchTest()
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

            // Act and assert
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
