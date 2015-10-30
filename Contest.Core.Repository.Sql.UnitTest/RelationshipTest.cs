using Contest.Core.Component;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class RelationshipTest
    {
        [TestFixtureSetUp]
        public void Init()
        {
            FlippingContainer.Instance.Current = new NoComposer();
        }

        [TestCase]
        public void Create()
        {
            var first = new Entity1();
            var second = new Entity2();
            var relationship = new Relationship<Entity1,Entity2>(first, second);

            Assert.AreEqual(first, relationship.FirstItemInvolve);
            Assert.AreEqual(first.Id, relationship.FirstItemInvolveId);
            Assert.AreEqual(second, relationship.SecondItemInvolve);
            Assert.AreEqual(second.Id, relationship.SecondItemInvolveId);
        }

        [TestCase]
        public void GettingFromRepo()
        {
            var first = new Entity1();
            var second = new Entity2();
            var relationship = new Relationship<Entity1, Entity2>();
            var repositoryMock1 = new SqlRepositoryMock<Entity1>();
            repositoryMock1.Add(first);
            relationship.FirstItemRepository = repositoryMock1;
            var repositoryMock2 = new SqlRepositoryMock<Entity2>();
            repositoryMock2.Add(second);
            relationship.SecondItemRepository = repositoryMock2;

            Assert.AreEqual(first, relationship.FirstItemInvolve);
            Assert.AreEqual(first.Id, relationship.FirstItemInvolveId);
            Assert.AreEqual(second, relationship.SecondItemInvolve);
            Assert.AreEqual(second.Id, relationship.SecondItemInvolveId);
        }
    }
}
