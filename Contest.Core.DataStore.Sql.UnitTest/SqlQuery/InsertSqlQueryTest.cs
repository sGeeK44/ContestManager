using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class InsertSqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_OverrideNameEntity_ShouldReturnCorrectStatement()
        {
            var query = CreateInsertSqlQuery(OverrideNameEntity.CreateMock());
            var expected = string.Format("INSERT INTO ENTITY_1 (ID, NAME, ACTIVE, AGE) VALUES ({0}, {0}, {0}, {0});", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        [TestCase]
        public void ToStatement_OverrideNameEntityWithInterface_ShouldReturnCorrectStatement()
        {
            var query = CreateInsertSqlQuery<OverrideNameEntity, IOverrideNameEntity>(OverrideNameEntity.CreateMock());
            var expected = string.Format("INSERT INTO ENTITY_1 (ID, NAME, ACTIVE, AGE) VALUES ({0}, {0}, {0}, {0});", ConvertedValue);

            Assert.AreEqual(expected, query.ToStatement());
        }

        public InsertSqlQuery<T, T> CreateInsertSqlQuery<T>(T item)
            where T : class
        {
            return CreateInsertSqlQuery<T, T>(item);
        }

        public InsertSqlQuery<T, TI> CreateInsertSqlQuery<T, TI>(TI item)
            where T : class
        {
            return new InsertSqlQuery<T, TI>(SqlStrategy.Object, EntityInfoFactory, item);
        }
    }
}
