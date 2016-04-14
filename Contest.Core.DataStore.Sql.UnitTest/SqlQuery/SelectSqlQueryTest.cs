using System;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class SelectSqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_TruePredicate_ShouldReturnSqlQueryWithoutWhereClause()
        {
            var query = CreateSelectSqlQuery<OverrideNameEntity>(_ => true);
            Assert.AreEqual("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1;", query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntity_ShouldReturnSqlQueryWithoutWhereClause()
        {
            var ent = OverrideNameEntity.CreateMock();
            var query = CreateSelectSqlQuery<OverrideNameEntity>(_ => _.Id == ent.Id);
            var expected = string.Format("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_ValidEntityWithInterface_ShouldReturnSqlQueryWithoutWhereClause()
        {
            IOverrideNameEntity ent = OverrideNameEntity.CreateMock();
            var query = CreateSelectSqlQuery<OverrideNameEntity, IOverrideNameEntity>(_ => _.Id == ent.Id);
            var expected = string.Format("SELECT ID, NAME, ACTIVE, AGE FROM ENTITY_1 WHERE ID = {0};", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        public SelectSqlQuery<T, T> CreateSelectSqlQuery<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            return CreateSelectSqlQuery<T, T>(predicate);
        }

        public SelectSqlQuery<T, TI> CreateSelectSqlQuery<T, TI>(Expression<Func<TI, bool>> predicate)
            where T : class, TI
            where TI : class
        {
            return new SelectSqlQuery<T, TI>(SqlStrategy.Object, predicate);
        }
    }
}
