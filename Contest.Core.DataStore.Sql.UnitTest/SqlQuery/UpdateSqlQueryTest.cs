using System;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class UpdateSqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_EntityWithoutPrimaryKey_ShouldThrowException()
        {
            var query = CreateUpdateSqlQuery<WithoutPrimaryKeyEntity, WithoutPrimaryKeyEntity>(WithoutPrimaryKeyEntity.CreateMock());
            Assert.Throws<NotSupportedException>(() => query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntity_ShouldReturnCorrectStatement()
        {
            var query = CreateUpdateSqlQuery(OverrideNameEntity.CreateMock());
            var expected = string.Format("UPDATE ENTITY_1 SET NAME = {0}, ACTIVE = {0}, AGE = {0} WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntityWithInterface_ShouldReturnCorrectStatement()
        {
            var query = CreateUpdateSqlQuery<OverrideNameEntity, IOverrideNameEntity>(OverrideNameEntity.CreateMock());
            var expected = string.Format("UPDATE ENTITY_1 SET NAME = {0}, ACTIVE = {0}, AGE = {0} WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        public UpdateSqlQuery<T, T> CreateUpdateSqlQuery<T>(T item)
            where T : class
        {
            return CreateUpdateSqlQuery<T, T>(item);
        }

        public UpdateSqlQuery<T, TI> CreateUpdateSqlQuery<T, TI>(TI item)
            where T : class
        {
            return new UpdateSqlQuery<T, TI>(SqlStrategy.Object, item);
        }
    }
}
