using System;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlOneToManyReferenceTest
    {
        [TestCase]
        public void GetSqlField_OneToManyEntity_AllMarkedProperty()
        {
            var fields = EntityInfoFactory.GetOneToManySqlReference<OneToManyEntity>();
            Assert.AreEqual(1, fields.Count);
        }

        [TestCase]
        public void GetSqlField_ReferencePropertiesIsNotAIList_ShouldThrowException()
        {
            Assert.Throws<NotSupportedException>(() => EntityInfoFactory.GetOneToManySqlReference<OneToManyWithoutIListEntity>());
        }

        [TestCase]
        public void GetSqlField_Key_ShouldEqualToExpectedProperty()
        {
            var expected = typeof(OneToManyEntity).GetProperty("Id");
            var fields = SqlOneToManyReferenceInfoTester.GetSqlReferenceTester<OneToManyEntity>();
            Assert.AreEqual(expected, fields[0].KeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlField_ReferenceKey_ShouldEqualToExpectedProperty()
        {
            var expected = typeof(ManyToOneEntity).GetProperty("OneToManyEntityId");
            var fields = SqlOneToManyReferenceInfoTester.GetSqlReferenceTester<OneToManyEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceKeyTester[0].PropertyInfo);
        }

        [TestCase]
        public void GetSqlField_ReferenceType_ShouldEqualToExpectedType()
        {
            var expected = typeof(ManyToOneEntity);
            var fields = SqlOneToManyReferenceInfoTester.GetSqlReferenceTester<OneToManyEntity>();
            Assert.AreEqual(expected, fields[0].ReferenceTypeTester);
        }
    }
}
