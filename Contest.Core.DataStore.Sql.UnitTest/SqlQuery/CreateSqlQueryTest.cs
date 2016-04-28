using Contest.Core.DataStore.Sql.SqlQuery;
using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    [TestFixture]
    public class CreateSqlQueryTest : SqlQueryTestBase
    {
        [TestCase]
        public void ToStatement_ValidEntity_ShouldReturnCorrectStatement()
        {
            var query = CreateCreateSqlQuery<OverrideNameEntity>();
            var expected = string.Format("CREATE TABLE IF NOT EXISTS ENTITY_1 (ID {0} primary key, NAME {0}, ACTIVE {0}, AGE {0});", ColumnType);
            Assert.AreEqual(expected, query.ToStatement());
        }
        
        public CreateSqlQuery<T> CreateCreateSqlQuery<T>()
            where T : class
        {
            return new CreateSqlQuery<T>(SqlStrategy.Object, EntityInfoFactory);
        }
    }
}
