using NUnit.Framework;
using System;
using Contest.Core.Component;
using Moq;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;

namespace Contest.Business.Fields.UnitTest
{
    [TestFixture]
    public class FieldTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var customComposer = new CustomComposer();
            customComposer.AddType(typeof(RepositoryContestMock));
            customComposer.AddType(typeof(RepositoryMatchMock));
            FlippingContainer.Instance.Current = customComposer;
        }

        [TestCase]
        public void ConstructorParamLess_AllFine_LazyShouldBeInitialize()
        {
            var field = new Field();
            Assert.IsNull(field.CurrentContest);
            Assert.IsNull(field.MatchInProgess);
        }

        [TestCase]
        public void Constructor_AllFine_PropertyShouldByInitialized()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");

            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(contest.Id, result.CurrentContestId);
            Assert.AreEqual(contest, result.CurrentContest);
            Assert.AreEqual(false, result.IsAllocated);
            Assert.AreEqual(null, result.MatchInProgess);
            Assert.AreEqual(Guid.Empty, result.MatchInProgessId);
            Assert.AreEqual("name", result.Name);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForSameMatch_MatchInProgressIdShouldEqualToSpecifiedMatchId()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();
            result.Allocate(match);

            result.Allocate(match);

            Assert.AreEqual(match.Id, result.MatchInProgessId);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForSameMatch_MatchInProgressShouldEqualToSpecifiedMatch()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();
            result.Allocate(match);

            result.Allocate(match);

            Assert.AreEqual(match, result.MatchInProgess);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForSameMatch_FieldIsAllocatedSouldBeTrue()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();
            result.Allocate(match);

            result.Allocate(match);

            Assert.IsTrue(result.IsAllocated);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForOtherMatch_ShouldTrowException()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();
            var otherMatch = CreateMatchStub();
            result.Allocate(match);

            Assert.Throws<ArgumentException>(() => result.Allocate(otherMatch));
        }

        [TestCase]
        public void Allocate_MatchNull_ShouldThrowException()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();

            Assert.Throws<ArgumentNullException>(() => result.Allocate(null));
        }

        [TestCase]
        public void Allocate_AllFine_MatchInProgressIdShouldEqualToSpecifiedMatchId()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();

            result.Allocate(match);
            
            Assert.AreEqual(match.Id, result.MatchInProgessId);
        }

        [TestCase]
        public void Allocate_AllFine_MatchInProgressShouldEqualToSpecifiedMatch()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();

            result.Allocate(match);
            
            Assert.AreEqual(match, result.MatchInProgess);
        }

        [TestCase]
        public void Allocate_AllFine_FieldIsAllocatedSouldBeTrue()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();

            result.Allocate(match);

            Assert.IsTrue(result.IsAllocated);
        }

        [TestCase]
        public void Release_NotAllocated_IsAllocatedShouldBeFalse()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
         
            result.Release();

            Assert.IsFalse(result.IsAllocated);
        }

        [TestCase]
        public void Release_Allocated_IsAllocatedShouldBeFalse()
        {
            var contest = CreateContestStub();
            var result = new Field(contest, "name");
            var match = CreateMatchStub();
            result.Allocate(match);

            result.Release();

            Assert.IsFalse(result.IsAllocated);
        }

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new Field(new Mock<IContest>().Object, "name");

            Assert.Throws<ArgumentNullException>(() => field.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var field = new Field(new Mock<IContest>().Object, "name");
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IField>(field)).Verifiable();

            field.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new Field(new Mock<IContest>().Object, "name");

            Assert.Throws<ArgumentNullException>(() => field.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var field = new Field(new Mock<IContest>().Object, "name");

            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IField>(field)).Verifiable();

            field.PrepareDelete(repoMock.Object);
        }

        private static IMatch CreateMatchStub()
        {
            var match = new Mock<IMatch>();
            match.Setup(_ => _.Id).Returns(Guid.NewGuid());
            return match.Object;
        }

        private static IContest CreateContestStub()
        {
            var stub = new Mock<IContest>();
            stub.Setup(_ => _.Id).Returns(Guid.NewGuid());
            return stub.Object;
        }
    }
}