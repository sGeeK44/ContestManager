using System;
using Contest.Core.DataStore.Sql.SqlQuery;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    public class SqlQueryTester<T> : SqlQuery<T>
    {
        public SqlQueryTester(ISqlProviderStrategy sqlProviderStrategy) : base(sqlProviderStrategy)
        {
        }

        public override string ToStatement()
        {
            throw new NotImplementedException();
        }
    }
}
