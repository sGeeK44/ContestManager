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
            var contest = CreateReadyToStartContest();
            var qualificationSetting = new Mock<IQualificationStepSetting>();
            contest.QualificationSetting = qualificationSetting.Object;

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

        private static void RegisterTeam(IContest contest, int countTeamToRegister)
        {
            for (var i = 1; i <= countTeamToRegister; i++)
                contest.Register(CreateTeamStub(i).Object);
        }

        private static Mock<ITeam> CreateTeamStub(int num)
        {
            var team = new Mock<ITeam>();
            team.Setup(_ => _.Id).Returns(new Guid("00000000-0000-0000-0000-00000000000" + num));
            team.Setup(_ => _.Name).Returns("Team " + num);
            return team;
        }

        private static ITeam CreateTeamStub1()
        {
            return CreateTeamStub(1).Object;
        }

        private static ITeam CreateTeamStub2()
        {
            return CreateTeamStub(2).Object;
        }

        private static ITeam CreateTeamStub3()
        {
            return CreateTeamStub(3).Object;
        }

        private static ITeam CreateTeamStub4()
        {
            return CreateTeamStub(4).Object;
        }

        private static ITeam CreateTeamStub5()
        {
            return CreateTeamStub(5).Object;
        }

        private static ITeam CreateTeamStub6()
        {
            return CreateTeamStub(6).Object;
        }

        private static ITeam CreateTeamStub7()
        {
            return CreateTeamStub(7).Object;
        }

        private static ITeam CreateTeamStub8()
        {
            return CreateTeamStub(8).Object;
        }

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var address = new Contest();

            Assert.Throws<ArgumentNullException>(() => address.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var address = new Contest();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IContest>(address)).Verifiable();

            address.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var address = new Contest();

            Assert.Throws<ArgumentNullException>(() => address.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var address = new Contest();
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IContest>(address)).Verifiable();

            address.PrepareDelete(repoMock.Object);
        }
    }
}