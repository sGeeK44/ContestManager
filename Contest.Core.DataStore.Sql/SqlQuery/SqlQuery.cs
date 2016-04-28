using System;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public abstract class SqlQuery<T> : ISqlQuery
    {
        private readonly Lazy<string> _tableName = new Lazy<string>(GetTableName);

        protected ISqlProviderStrategy SqlProviderStrategy { get; set; }
    
        protected SqlQuery(ISqlProviderStrategy sqlProviderStrategy)
        {
            if (sqlProviderStrategy == null) throw new ArgumentNullException("sqlProviderStrategy");

            SqlProviderStrategy = sqlProviderStrategy;
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

        private static string GetTableName()
        {
            return EntityInfoFactory.GetEntityInfo<T>().TableName;
        }
    }
}
