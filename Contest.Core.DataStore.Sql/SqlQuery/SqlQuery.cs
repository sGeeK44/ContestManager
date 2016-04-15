using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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
            var tableAttribute = typeof(T).GetCustomAttributes(typeof(DataContractAttribute), true)
                                          .Cast<DataContractAttribute>()
                                          .FirstOrDefault();

            if (tableAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no DataContract attribute.", typeof(T)));

            return tableAttribute.Name ?? typeof(T).Name;
        }
    }
}
