using System;
using Contest.Core.DataStore.Sql.EntityInfo;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public abstract class SqlQuery<T> : ISqlQuery
    {
        private readonly Lazy<string> _tableName;

        protected ISqlProviderStrategy SqlProviderStrategy { get; set; }
        protected IEntityInfoFactory EntityInfoFactory { get; set; }

        protected SqlQuery(ISqlProviderStrategy sqlProviderStrategy, IEntityInfoFactory entityInfoFactory)
        {
            if (sqlProviderStrategy == null) throw new ArgumentNullException("sqlProviderStrategy");
            if (entityInfoFactory == null) throw new ArgumentNullException("entityInfoFactory");

            SqlProviderStrategy = sqlProviderStrategy;
            EntityInfoFactory = entityInfoFactory;

            _tableName = new Lazy<string>(() => EntityInfoFactory.GetEntityInfo<T>().TableName);
        }

        /// <summary>
        /// Get table name involve in query
        /// </summary>
        public string TableName { get { return _tableName.Value; } }

        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        public abstract string ToStatement();
    }
}
