using System;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using Contest.Domain.Settings;
using Moq;
using NUnit.Framework;

namespace Contest.Business.UnitTest
{
    [TestFixture]
    public class FieldTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var customComposer = new CustomComposer();
            customComposer.AddType(typeof(ContestRepositoryMock));
            customComposer.AddType(typeof(MatchRepositoryMock));
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

            Assert.AreEqual(Guid.Empty, result.Id);
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

        private static IMatch CreateMatchStub()
        {
            var match = new Mock<IMatch>();
            match.Setup(_ => _.Id).Returns(Guid.NewGuid());
            match.Setup(_ => _.GetPkValue()).Returns(match.Object.Id);
            return match.Object;
        }

        private static IContest CreateContestStub()
        {
            var stub = new Mock<IContest>();
            stub.Setup(_ => _.Id).Returns(Guid.NewGuid());
            stub.Setup(_ => _.GetPkValue()).Returns(stub.Object.Id);
            return stub.Object;
        }
    }
}