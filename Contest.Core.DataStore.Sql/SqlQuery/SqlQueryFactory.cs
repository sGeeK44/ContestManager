using System;
using System.Linq.Expressions;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class SqlQueryFactory<T> : SqlQueryFactory<T, T>
        where T : class
    {
        public SqlQueryFactory(ISqlProviderStrategy sqlProviderStrategy)
            : base(sqlProviderStrategy) { }
    }

    public class SqlQueryFactory<T, TI> : ISqlQueryFactory<TI>
        where T : class, TI
        where TI : class
    {
        private ISqlProviderStrategy SqlProviderStrategy { get; set; }

        public SqlQueryFactory(ISqlProviderStrategy sqlProviderStrategy)
        {
            SqlProviderStrategy = sqlProviderStrategy;
        }

        public ISqlQuery CreateTable()
        {
            return new CreateSqlQuery<T>(SqlProviderStrategy);
        }

        public ISqlQuery Select(Expression<Func<TI, bool>> predicate)
        {
            return new SelectSqlQuery<T, TI>(SqlProviderStrategy, predicate);
        }

        public ISqlQuery Insert(TI item)
        {
            return new InsertSqlQuery<T, TI>(SqlProviderStrategy, item);
        }

        public ISqlQuery Update(TI item)
        {
            return new UpdateSqlQuery<T, TI>(SqlProviderStrategy, item);
        }

        public ISqlQuery Delete(TI item)
        {
            return new DeleteSqlQuery<T, TI>(SqlProviderStrategy, item);
        }
    }
}