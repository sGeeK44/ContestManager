using NUnit.Framework;
using System;
using Contest.Core.Component;
using Moq;
using Contest.Core.Repository.Sql;

namespace Contest.Business.Fields.UnitTest
{
    [TestFixture]
    public class FieldTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            FlippingContainer.Instance.Current = new ExecutingAssemblies();
        }

        [TestCase]
        public void Constructor_AllFine_PropertyShouldByInitialized()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
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
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            result.Allocate(match);

            Assert.AreEqual(match.Id, result.MatchInProgessId);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForSameMatch_MatchInProgressShouldEqualToSpecifiedMatch()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            result.Allocate(match);

            Assert.AreEqual(match, result.MatchInProgess);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForSameMatch_FieldIsAllocatedSouldBeTrue()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            result.Allocate(match);

            Assert.IsTrue(result.IsAllocated);
        }

        [TestCase]
        public void Allocate_FieldAlreadyAllocatedForOtherMatch_ShouldTrowException()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            var otherMatch = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            Assert.Throws<ArgumentException>(() => result.Allocate(otherMatch));
        }

        [TestCase]
        public void Allocate_MatchNull_ShouldThrowException()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            Assert.Throws<ArgumentNullException>(() => result.Allocate(null));
        }

        [TestCase]
        public void Allocate_AllFine_MatchInProgressIdShouldEqualToSpecifiedMatchId()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            result.Allocate(match);
            
            Assert.AreEqual(match.Id, result.MatchInProgessId);
        }

        [TestCase]
        public void Allocate_AllFine_MatchInProgressShouldEqualToSpecifiedMatch()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            result.Allocate(match);
            
            Assert.AreEqual(match, result.MatchInProgess);
        }

        [TestCase]
        public void Allocate_AllFine_FieldIsAllocatedSouldBeTrue()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            result.Allocate(match);

            Assert.IsTrue(result.IsAllocated);
        }

        [TestCase]
        public void PrepareCommit_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new Field(new ContestMock(), "name");

            Assert.Throws<ArgumentNullException>(() => field.PrepareCommit(null));
        }

        [TestCase]
        public void PrepareCommit_ValidISqlUnitOfWorks_ShouldInsertObject()
        {
            var field = new Field(new ContestMock(), "name");
            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.InsertOrUpdate<IField>(field)).Verifiable();

            field.PrepareCommit(repoMock.Object);
        }
        [TestCase]
        public void PrepareDelete_NullISqlUnitOfWorks_ShouldThrowException()
        {
            var field = new Field(new ContestMock(), "name");

            Assert.Throws<ArgumentNullException>(() => field.PrepareDelete(null));
        }

        [TestCase]
        public void PrepareDelete_ValidISqlUnitOfWorks_ShouldDeleteObject()
        {
            var field = new Field(new ContestMock(), "name");

            var repoMock = new Mock<ISqlUnitOfWorks>(MockBehavior.Strict);
            repoMock.Setup(_ => _.Delete<IField>(field)).Verifiable();

            field.PrepareDelete(repoMock.Object);
        }
    }
}