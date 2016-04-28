using System.Linq.Expressions;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class SqlQueryFactory<T> : SqlQueryFactory<T, T>
        where T : class
    {
        public SqlQueryFactory(ISqlProviderStrategy sqlProviderStrategy, IEntityInfoFactory entityInfoFactory)
            : base(sqlProviderStrategy, entityInfoFactory) { }
    }

    public class SqlQueryFactory<T, TI> : ISqlQueryFactory<TI>
        where T : class, TI
        where TI : class
    {
        private ISqlProviderStrategy SqlProviderStrategy { get; set; }
        private IEntityInfoFactory EntityInfoFactory { get; set; }

        public SqlQueryFactory(ISqlProviderStrategy sqlProviderStrategy, IEntityInfoFactory entityInfoFactory)
        {
            SqlProviderStrategy = sqlProviderStrategy;
            EntityInfoFactory = entityInfoFactory;
        }

        public ISqlQuery CreateTable()
        {
            return new CreateSqlQuery<T>(SqlProviderStrategy, EntityInfoFactory);
        }

        public ISqlQuery Select(LambdaExpression predicate)
        {
            return new SelectSqlQuery<T>(SqlProviderStrategy, EntityInfoFactory, predicate);
        }

        public ISqlQuery Insert(TI item)
        {
            return new InsertSqlQuery<T, TI>(SqlProviderStrategy, EntityInfoFactory, item);
        }

        public ISqlQuery Update(TI item)
        {
            return new UpdateSqlQuery<T, TI>(SqlProviderStrategy, EntityInfoFactory, item);
        }

        public ISqlQuery Delete(TI item)
        {
            return new DeleteSqlQuery<T, TI>(SqlProviderStrategy, EntityInfoFactory, item);
        }
    }
}