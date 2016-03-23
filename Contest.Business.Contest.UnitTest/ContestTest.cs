using System;
using Contest.Core.Component;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;
using Moq;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class ContestTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var customComposer = new CustomComposer();
            customComposer.AddType(typeof(RepositoryContestMock));
            customComposer.AddType(typeof(RepositoryTeamMock));
            customComposer.AddType(typeof(RepositoryFieldMock));
            customComposer.AddType(typeof(RepositoryPhysicalSettingMock));
            customComposer.AddType(typeof(RepositoryGameStepMock));
            customComposer.AddType(typeof(RepositoryGameSettingMock));
            customComposer.AddType(typeof(RepositoryEliminationStepSettingMock));
            customComposer.AddType(typeof(RepositoryQualificationStepMock));
            customComposer.AddType(typeof(RepositoryQualificationStepSettingMock));
            customComposer.AddType(typeof(RepositoryEliminationStepMock));
            customComposer.AddType(typeof(RepositoryIRelationshipTeamGameStepMock));
            customComposer.AddType(typeof(RepositoryIRelationshipTeamPhaseMock));
            customComposer.AddType(typeof(RepositoryPhaseMock));
            customComposer.AddType(typeof(RepositoryMatchSettingMock));
            customComposer.AddType(typeof(RepositoryMatchMock));
            customComposer.AddType(typeof(MatchFactory));
            customComposer.AddType(typeof(FieldFactory));
            customComposer.AddType(typeof(TeamPhaseRelationshipFactory));
            customComposer.AddType(typeof(TeamGameStepRelationshipFactory));
            FlippingContainer.Instance.Current = customComposer;
        }

        [TestCase]
        public void PlannedContest_WithoutQualification_PhaseListShouldBeEmpty()
        {
            var contest = CreatePlannedContest();

            Assert.AreEqual(0, contest.PhaseList.Count);
        }

        [TestCase]
        public void PlannedContest_ValidArg_BeginningDateShouldBeNull()
        {
            var contest = CreatePlannedContest();

            Assert.IsNull(contest.BeginningDate);
        }

        [TestCase]
        public void PlannedContest_ValidArg_IsPlannededShouldReturnFalse()
        {
            var contest = CreatePlannedContest();

            Assert.IsFalse(contest.IsStarted);
        }

        [TestCase]
        public void PlannedContest_ValidArg_DatePlannedShouldBeNotNull()
        {
            var contest = CreatePlannedContest();

            Assert.IsNotNull(contest.DatePlanned);
        }

        [TestCase]
        public void PlannedContest_ValidArg_IsFinishedShouldReturnFalse()
        {
            var contest = CreatePlannedContest();

            Assert.IsFalse(contest.IsFinished);
        }

        [TestCase]
        public void Register_TeamNotRegister_TeamListShouldContainsOneElement()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();

            contest.Register(team.Object);

            Assert.AreEqual(1, contest.TeamList.Count);
        }

        [TestCase]
        public void Register_TeamAlreadyRegister_ShouldThowException()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();
            contest.Register(team.Object);

            Assert.Throws<ArgumentException>(() => contest.Register(team.Object));
        }

        [TestCase]
        public void UnRegister_TeamRegister_TeamListShouldContainsZeroElement()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();
            contest.Register(team.Object);

            contest.UnRegister(team.Object);

            Assert.AreEqual(0, contest.TeamList.Count);
        }

        [TestCase]
        public void UnRegister_TeamNoRegister_ShouldThowException()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();

            Assert.Throws<ArgumentException>(() => contest.UnRegister(team.Object));
        }

        [TestCase]
        public void Register_OtherTeamWithSameNameAlreadyRegister_ShouldThowException()
        {
            var contest = CreatePlannedContest();
            var team1 = new Mock<ITeam>();
            team1.Setup(_ => _.Name).Returns("Name");
            contest.Register(team1.Object);
            var team2 = new Mock<ITeam>();
            team2.Setup(_ => _.Name).Returns("name");

            Assert.Throws<ArgumentException>(() => contest.Register(team2.Object));
        }

        [TestCase]
        public void IsRegister_TeamNotRegister_ShouldReturnFalse()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();

            Assert.IsFalse(contest.IsRegister(team.Object));
        }

        [TestCase]
        public void IsRegister_TeamRegistered_ShouldReturnTrue()
        {
            var contest = CreatePlannedContest();
            var team = new Mock<ITeam>();
            contest.Register(team.Object);

            Assert.IsTrue(contest.IsRegister(team.Object));
        }

        [TestCase]
        public void StartContest_With10Field_FieldShouldContains10Occurs()
        {
            var physicalSetting = new Mock<IPhysicalSetting>();
            physicalSetting.Setup(_ => _.CountField).Returns(10);
            var contest = CreateReadyToStartContest(physicalSetting: physicalSetting);

            contest.StartContest();

            Assert.AreEqual(10, contest.FieldList.Count);
        }

        [TestCase]
        public void StartContest_WithQualification_PhaseListShouldContainsnewQualificationPhase()
        {
            var contest = CreateReadyToStartContestWithQualification();

            contest.StartContest();

            Assert.AreEqual(1, contest.PhaseList.Count);
            Assert.AreEqual(PhaseType.Qualification, contest.PhaseList[0].Type);
        }

        [TestCase]
        public void StartContest_WithoutQualification_PhaseListShouldContainsNewMainPhase()
        {
            var contest = CreateReadyToStartContest();

            contest.StartContest();

            Assert.AreEqual(1, contest.PhaseList.Count);
            Assert.AreEqual(PhaseType.Main, contest.PhaseList[0].Type);
        }

        [TestCase]
        public void StartContest_ValidArg_BeginningDateShouldBeNotNull()
        {
            var contest = CreateReadyToStartContest();

            contest.StartContest();

            Assert.IsNotNull(contest.BeginningDate);
        }

        [TestCase]
        public void StartContest_ValidArg_StartContestShouldBeRaise()
        {
            var contest = CreateReadyToStartContest();
            var startEventIsRaised = 0;
            contest.ContestStart += sender => startEventIsRaised++;

            contest.StartContest();

            Assert.AreEqual(1, startEventIsRaised);
        }

        [TestCase]
        public void StartContest_ValidArg_IsStartedShouldReturnTrue()
        {
            var contest = CreateReadyToStartContest();

            contest.StartContest();

            Assert.IsTrue(contest.IsStarted);
        }

        [TestCase]
        public void StartContest_ValidArg_IsFinishedShouldReturnFalse()
        {
            var contest = CreateReadyToStartContest();

            contest.StartContest();

            Assert.IsFalse(contest.IsFinished);
        }

        [TestCase]
        public void LaunchNextPhase_ContestWithoutQualificationPhase_ShouldThrowException()
        {
            var contest = CreatePlannedContest();

            Assert.Throws<NotSupportedException>(() => contest.LaunchNextPhase());
        }

        [TestCase]
        public void LaunchNextPhase_ContestWithQualificationNotStart_ShouldThrowException()
        {
            var contest = CreateReadyToStartContestWithQualification();

            Assert.Throws<NotSupportedException>(() => contest.LaunchNextPhase());
        }

        [TestCase]
        public void LaunchNextPhase_ContestWithQualificationPhaseNotEnded_ShouldThrowException()
        {
            var contest = CreateReadyToStartContestWithQualification();
            contest.StartContest();

            Assert.Throws<NotSupportedException>(() => contest.LaunchNextPhase());
        }

        [TestCase]
        public void LaunchNextPhase_ContestWithout_PrincipalPhaseShouldBeNotNull()
        {
            var contest = CreateContestWithQualificationReadyToEnd();

            contest.LaunchNextPhase();

            Assert.IsNotNull(contest.PrincipalPhase);
        }

        [TestCase]
        public void LaunchNextPhase_ContestWithout_ConsolingPhaseShouldBeNull()
        {
            var contest = CreateContestWithQualificationReadyToEnd();

            contest.LaunchNextPhase();

            Assert.IsNull(contest.ConsolingPhase);
        }

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var contest = new Contest();

            Assert.Throws<ArgumentNullException>(() => contest.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var contest = new Contest();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IContest>(contest)).Verifiable();

            contest.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var contest = new Contest();

            Assert.Throws<ArgumentNullException>(() => contest.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var contest = new Contest();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IContest>(contest)).Verifiable();

            contest.PrepareDelete(repoMock.Object);
        }

        private static IContest CreatePlannedContest(Mock<IPhysicalSetting> physicalSetting = null, Mock<IGameSetting> gameSetting = null)
        {
            physicalSetting = physicalSetting ?? new Mock<IPhysicalSetting>();
            gameSetting = gameSetting ?? new Mock<IGameSetting>();
            return Contest.Create(DateTime.Now, physicalSetting.Object, gameSetting.Object);
        }

        private static IContest CreateReadyToStartContest(int countTeamRegister = 8, Mock<IPhysicalSetting> physicalSetting = null, Mock<IGameSetting> gameSetting = null)
        {
            var contest = CreatePlannedContest(physicalSetting, gameSetting);
            RegisterTeam(contest, countTeamRegister);

            return contest;
        }

        private static IContest CreateReadyToStartContestWithQualification(int countTeamRegister = 16, Mock<IPhysicalSetting> physicalSetting = null, Mock<IGameSetting> gameSetting = null, Mock<IQualificationStepSetting> qualificationSetting = null)
        {
            var result = CreateReadyToStartContest(countTeamRegister, physicalSetting, gameSetting);
            qualificationSetting = qualificationSetting ?? GetDefaultQualificationSettingMock();
            result.QualificationSetting = qualificationSetting.Object;
            return result;
        }

        private static IContest CreateContestWithQualificationReadyToEnd(int countTeamRegister = 16, Mock<IPhysicalSetting> physicalSetting = null, Mock<IGameSetting> gameSetting = null, Mock<IQualificationStepSetting> qualificationSetting = null)
        {            
            var result = CreateReadyToStartContestWithQualification(countTeamRegister, physicalSetting, gameSetting, qualificationSetting);
            result.StartContest();
            foreach(var gameStep in result.QualificationPhase.GameStepList)
            {
                foreach(var match in gameStep.MatchList)
                {
                    match.Start(new Mock<IField>().Object);
                    match.SetResult(1, 0);
                }
            }
            return result;
        }

        private static Mock<IQualificationStepSetting> GetDefaultQualificationSettingMock()
        {
            var result = new Mock<IQualificationStepSetting>();
            result.Setup(_ => _.CountGroup).Returns(2);
            result.Setup(_ => _.CountTeamFished).Returns(0);
            result.Setup(_ => _.CountTeamQualified).Returns(4);
            result.Setup(_ => _.MatchSetting).Returns(GetDefaultMatchSetting().Object);
            return result;
        }

        private static Mock<IMatchSetting> GetDefaultMatchSetting()
        {
            var result = new Mock<IMatchSetting>();
            result.Setup(_ => _.MatchPoint).Returns(1);
            string mess;
            result.Setup(_ => _.IsValidScore(It.IsAny<ushort>(), out mess)).Returns(true);
            result.Setup(_ => _.IsValidToFinishedMatch(It.IsAny<ushort>(), It.IsAny<ushort>(), out mess)).Returns(true);
            return result;
        }

        private static void RegisterTeam(IContest contest, int countTeamToRegister)
        {
            for (var i = 1; i <= countTeamToRegister; i++)
                contest.Register(CreateTeamStub(i).Object);
        }

        private static Mock<ITeam> CreateTeamStub(int num)
        {
            var team = new Mock<ITeam>();
            team.Setup(_ => _.Id).Returns(new Guid("00000000-0000-0000-0000-0000000000" + num.ToString("D2")));
            team.Setup(_ => _.Name).Returns("Team " + num);
            return team;
        }
    }
}