using System;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using Contest.Core.Repository;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.ReferenceManyToMany
{
    [TestFixture]
    public class RelationshipTest
    {
        [TestCase]
        public void Create()
        {
            var first = new FirstManyToManyEntity();
            var second = new SecondManyToManyEntity();
            var relationship = new Relationship<FirstManyToManyEntity, FirstManyToManyEntity, SecondManyToManyEntity, SecondManyToManyEntity>(first, second);

            Assert.AreEqual(first, relationship.FirstItemInvolve);
            Assert.AreEqual(first.Id, relationship.FirstItemInvolveId);
            Assert.AreEqual(second, relationship.SecondItemInvolve);
            Assert.AreEqual(second.Id, relationship.SecondItemInvolveId);
        }

        [TestCase]
        public void GettingFromRepo()
        {
            var first = new FirstManyToManyEntity();
            var second = new SecondManyToManyEntity();
            var relationship = new Relationship<FirstManyToManyEntity, FirstManyToManyEntity, SecondManyToManyEntity, SecondManyToManyEntity>();
            var repositoryMock1 = new Mock<IRepository<FirstManyToManyEntity>>();
            repositoryMock1.Setup(_ => _.FirstOrDefault(It.IsAny<Expression<Func<FirstManyToManyEntity, bool>>>())).Returns(first);
            relationship.FirstItemRepository = repositoryMock1.Object;

            var repositoryMock2 = new Mock<IRepository<SecondManyToManyEntity>>();
            repositoryMock2.Setup(_ => _.FirstOrDefault(It.IsAny<Expression<Func<SecondManyToManyEntity, bool>>>())).Returns(second);
            relationship.SecondItemRepository = repositoryMock2.Object;

            Assert.AreEqual(first, relationship.FirstItemInvolve);
            Assert.AreEqual(first.Id, relationship.FirstItemInvolveId);
            Assert.AreEqual(second, relationship.SecondItemInvolve);
            Assert.AreEqual(second.Id, relationship.SecondItemInvolveId);
        }
    }
}
