using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Contest.Core.Component;
using Contest.Core.Repository.Sql;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Contest.Business.Fields.UnitTest
{
    [TestClass()]
    public class FieldTest
    {
        [TestInitialize]
        public void Init()
        {
            FlippingContainer.Instance.Current = new ExecutingAssemblies();
        }

        [TestMethod()]
        public void NewTest()
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

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AllocateTest_OtherMatchFieldAlreadyAllocated()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            var otherMatch = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            result.Allocate(otherMatch);
        }

        [TestMethod()]
        public void AllocateTest_SameMatchFieldAlreadyAllocated()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };
            var otherMatch = new MatchMock() { Id = Guid.NewGuid() };
            result.Allocate(match);

            result.Allocate(match);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AllocateTest_MatchNull()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            result.Allocate(null);
        }

        [TestMethod()]
        public void AllocateTest_AllFine()
        {
            var contest = new ContestMock() { Id = Guid.NewGuid() };
            var result = new Field(contest, "name");
            var match = new MatchMock() { Id = Guid.NewGuid() };

            result.Allocate(match);

            Assert.IsTrue(result.IsAllocated);
            Assert.AreEqual(match, result.MatchInProgess);
            Assert.AreEqual(match.Id, result.MatchInProgessId);
        }

        [TestMethod()]
        public void PrepareCommitTest()
        {
            Assert.Fail();
        }
    }
}