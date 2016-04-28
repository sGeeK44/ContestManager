using System;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class DeleteSqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_EntityWithoutPrimaryKey_ShouldThrowException()
        {
            var query = CreateDeleteSqlQuery<WithoutPrimaryKeyEntity, WithoutPrimaryKeyEntity>(WithoutPrimaryKeyEntity.CreateMock());
            Assert.Throws<NotSupportedException>(() => query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntity_ShouldReturnCorrectStatement()
        {
            var query = CreateDeleteSqlQuery(OverrideNameEntity.CreateMock());
            var expected = string.Format("DELETE FROM ENTITY_1 WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntityWithInterface_ShouldReturnCorrectStatement()
        {
            var query = CreateDeleteSqlQuery<OverrideNameEntity, IOverrideNameEntity>(OverrideNameEntity.CreateMock());
            var expected = string.Format("DELETE FROM ENTITY_1 WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        public DeleteSqlQuery<T, T> CreateDeleteSqlQuery<T>(T item)
            where T : class
        {
            return CreateDeleteSqlQuery<T, T>(item);
        }

        public DeleteSqlQuery<T, TI> CreateDeleteSqlQuery<T, TI>(TI item)
            where T : class
        {
            return new DeleteSqlQuery<T, TI>(SqlStrategy.Object, EntityInfoFactory, item);
        }
    }
}
