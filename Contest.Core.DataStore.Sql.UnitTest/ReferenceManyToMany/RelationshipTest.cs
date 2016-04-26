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
        public void Constructor_WithItemRelation_ShouldFillFirstItemProp()
        {
            var first = new ManyToManyFirstEntity();
            var second = new ManyToManySecondEntity();

            var relationship = new Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>(first, second);

            Assert.AreEqual(first, relationship.FirstItemInvolve);
        }

        [TestCase]
        public void Constructor_WithItemRelation_ShouldFillFirstItemIdProp()
        {
            var first = new ManyToManyFirstEntity();
            var second = new ManyToManySecondEntity();

            var relationship = new Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>(first, second);

            Assert.AreEqual(first.Id, relationship.FirstItemInvolveId);
        }

        [TestCase]
        public void Constructor_WithItemRelation_ShouldFillSecondItemProp()
        {
            var first = new ManyToManyFirstEntity();
            var second = new ManyToManySecondEntity();

            var relationship = new Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>(first, second);

            Assert.AreEqual(second, relationship.SecondItemInvolve);
        }

        [TestCase]
        public void Constructor_WithItemRelation_ShouldFillSecondItemIdProp()
        {
            var first = new ManyToManyFirstEntity();
            var second = new ManyToManySecondEntity();

            var relationship = new Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>(first, second);

            Assert.AreEqual(second.Id, relationship.SecondItemInvolveId);
        }
    }
}
