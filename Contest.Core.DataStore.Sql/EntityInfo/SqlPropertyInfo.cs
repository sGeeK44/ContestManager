using System;
using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public abstract class SqlPropertyInfo : ISqlPropertyInfo
    {
        /// <summary>
        /// Get associated property
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Get sql column name
        /// </summary>
        public abstract string ColumnName { get; }

        /// <summary>
        /// Indicate if current property is a primary key
        /// </summary>
        public abstract bool IsPrimaryKey { get; }

        /// <summary>
        /// Indicate if current property is a foreign key of specified property
        /// </summary>
        /// <param name="prop">ManyToOne property</param>
        /// <returns>True if current part is foreign key, false else</returns>
        public abstract bool IsForeignKeyOf(PropertyInfo prop);

        /// <summary>
        /// Convert property value on specified entity to sql string value equivalent
        /// </summary>
        /// <param name="sqlProviderStrategy">Strategy used to converter value</param>
        /// <param name="entity">Entity on wich getting property value</param>
        /// <returns>Sql string value equivalent</returns>
        public abstract string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object entity);

        protected SqlPropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        protected void SetValue(object objectToSet, object innerValue)
        {
            if (!PropertyInfo.CanWrite) throw new NotSupportedException(string.Format("You have flags property as SqlField, but setter isn't accessible. Property involve: {0} ({1}).", PropertyInfo.Name, PropertyInfo.PropertyType));
            if (innerValue != null) PropertyInfo.SetValue(objectToSet, innerValue, null);
        }
    }
}
