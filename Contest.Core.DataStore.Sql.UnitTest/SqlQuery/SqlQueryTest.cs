using System;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class SqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void Constructor_WithNullSqlStrategy_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlQueryTester<object>(null, null));
        }

        [TestCase]
        public void Constructor_WithNullEntityInfoFactory_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => new SqlQueryTester<object>(SqlStrategy.Object, null));
        }

        [TestCase]
        public void TableName_WithoutDataMemberAttr_ShouldThrowException()
        {
            var query = CreateSqlQuery<NoDataContractEntity>();
            Assert.Throws<NotSupportedException>(() => { var tmp = query.TableName; });
        }

        [TestCase]
        public void TableName_WithNameOnDataMemberAttr_ShouldReturnName()
        {
            var query = CreateSqlQuery<OverrideNameEntity>();
            Assert.AreEqual("ENTITY_1", query.TableName);
        }

        [TestCase]
        public void TableName_WithNoNameOnDataMemberAttr_ShouldReturnClassName()
        {
            var query = CreateSqlQuery<NoOverrideNameEntity>();
            Assert.AreEqual("NoOverrideNameEntity", query.TableName);
        }

        public SqlQueryTester<T> CreateSqlQuery<T>()
            where T : class
        {
            return new SqlQueryTester<T>(SqlStrategy.Object, EntityInfoFactory);
        }
    }
}
