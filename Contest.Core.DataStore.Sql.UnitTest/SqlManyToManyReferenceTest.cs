using System.Linq;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlManyToManyReferenceTest
    {
        [TestCase]
        public void GetSqlReference_ManyToManyFirstEntity_AllMarkedProperty()
        {
            var fields = EntityInfoFactory.GetManyToManySqlReference<ManyToManyFirstEntity>();
            Assert.AreEqual(1, fields.Count);
        }

        [TestCase]
        public void GetSqlReference_FirstEntity_KeyShouldEqualToExpectedProperty()
        {
            var expected = typeof(ManyToManyFirstEntity).GetProperty("Id");
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManyFirstEntity>();
            Assert.AreEqual(expected, fields[0].KeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlReference_SecondEntity_KeyShouldEqualToExpectedProperty()
        {
            var expected = typeof(ManyToManySecondEntity).GetProperty("Id");
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManySecondEntity>();
            Assert.AreEqual(expected, fields[0].KeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlReference_FirstEntity_ReferenceKeyShouldEqualToExpectedProperty()
        {
            var expected = typeof(Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>).GetProperty("FirstItemInvolveId");
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManyFirstEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceKeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlReference_SecondEntity_ReferenceKeyShouldEqualToExpectedProperty()
        {
            var expected = typeof(Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>).GetProperty("SecondItemInvolveId");
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManySecondEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceKeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlReference_FirstEntity_ReferenceTypeShouldEqualToExpectedType()
        {
            var expected = typeof(Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>);
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManyFirstEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceTypeTester);
        }

        [TestCase]
        public void GetSqlReference_SecondEntity_ReferenceTypeShouldEqualToExpectedType()
        {
            var expected = typeof(Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>);
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManySecondEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceTypeTester);
        }

        [TestCase]
        public void GetPredicate_ManyToManyFirstEntity_ShouldReturnCorrectPredicate()
        {
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManyFirstEntity>().First();

            var result = fields.GetLambdaExpression(ManyToManyFirstEntity.Create());

            Assert.AreEqual("_ => (_.FirstItemInvolveId == value(Contest.Core.DataStore.Sql.UnitTest.Entities.ManyToManyFirstEntity).Id)", result.ToString());
        }

        [TestCase]
        public void GetPredicate_ManyToManySecondEntity_ShouldReturnCorrectPredicate()
        {
            var fields = SqlManyToManyReferenceInfoTester.GetSqlReferenceTester<ManyToManySecondEntity>().First();

            var result = fields.GetLambdaExpression(ManyToManySecondEntity.Create());

            Assert.AreEqual("_ => (_.SecondItemInvolveId == value(Contest.Core.DataStore.Sql.UnitTest.Entities.ManyToManySecondEntity).Id)", result.ToString());
        }
    }
}
