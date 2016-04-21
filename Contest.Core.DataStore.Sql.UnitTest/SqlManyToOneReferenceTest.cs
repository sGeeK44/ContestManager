using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlManyToOneReferenceTest
    {
        [TestCase]
        public void GetSqlField_ManyToOneEntity_AllMarkedProperty()
        {
            var fields = SqlManyToOneReferenceInfo.GetSqlReference<ManyToOneEntity>();
            Assert.AreEqual(1, fields.Count);
        }

        [TestCase]
        public void GetSqlField_Key_ShouldEqualToExpectedProperty()
        {
            var expected = typeof (ManyToOneEntity).GetProperty("OneToManyEntityId");
            var fields = SqlManyToOneReferenceInfoTester.GetSqlReferenceTester<ManyToOneEntity>();
            Assert.AreEqual(expected, fields[0].KeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlField_ReferenceKey_ShouldEqualToExpectedProperty()
        {
            var expected = typeof(OneToManyEntity).GetProperty("Id");
            var fields = SqlManyToOneReferenceInfoTester.GetSqlReferenceTester<ManyToOneEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceKeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlField_ReferenceType_ShouldEqualToExpectedType()
        {
            var expected = typeof(OneToManyEntity);
            var fields = SqlManyToOneReferenceInfoTester.GetSqlReferenceTester<ManyToOneEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceTypeTester);
        }
    }
}
