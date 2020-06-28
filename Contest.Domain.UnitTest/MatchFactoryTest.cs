using System;
using Contest.Domain.Matchs;
using NFluent;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class MatchFactoryTest : MatchTestBase
    {
        [Test]
        public void Create_NullGameStep_ShouldThrowException()
        {
            Check.ThatCode<IMatch>(() => new MatchFactory().Create(null, null, null, null)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Create_NullTeam1_ShouldThrowException()
        {
            var gameStep = CreateGameStepStub1();

            Check.ThatCode<IMatch>(() => new MatchFactory().Create(gameStep.Object, null, null, null)).Throws<ArgumentNullException>();
        }
        
        [Test]
        public void Create_NullTeam2_ShouldThrowException()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();

            Check.ThatCode<IMatch>(() => new MatchFactory().Create(gameStep.Object, team1.Object, null, null)).Throws<ArgumentNullException>();
        }

        [Test]
        public void Create_NullMatchSetting_ShouldThrowException()
        {
            // Arrange
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();

            // Act
            Check.ThatCode<IMatch>(() => new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, null)).Throws<ArgumentNullException>();
        }
        
        [Test]
        public void Create_SameTeam_ShouldThrowException()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub1();
            var matchSetting = CreateMatchSettingStub1();

            Check.ThatCode<IMatch>(() => new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object)).ThrowsAny();
        }
        
        [TestCase]
        public void Create_ValidArgument_ShouldReturnInitializedIMatch()
        {
            var gameStep = CreateGameStepStub1();
            var team1 = CreateTeamStub1();
            var team2 = CreateTeamStub2();
            var matchSetting = CreateMatchSettingStub1();

            var match = new MatchFactory().Create(gameStep.Object, team1.Object, team2.Object, matchSetting.Object);
            
            Assert.AreEqual(null, match.Beginning);
            Assert.AreEqual(null, match.Endded);
            Assert.AreEqual(gameStep.Object, match.GameStep);
            Assert.AreEqual(team1.Object, match.Team1);
            Assert.AreEqual(team2.Object, match.Team2);
            Assert.AreEqual(matchSetting.Object, match.Setting);
            Assert.AreEqual(false, match.IsBeginning);
            Assert.AreEqual(false, match.IsClose);
            Assert.AreEqual(false, match.IsFinished);
            Assert.AreEqual(null, match.MatchField);
            Assert.AreEqual(MatchState.Planned, match.MatchState);
            Assert.AreEqual(0, match.TeamScore1);
            Assert.AreEqual(0, match.TeamScore2);
            Assert.AreEqual(null, match.Winner);
        }
    }
}
