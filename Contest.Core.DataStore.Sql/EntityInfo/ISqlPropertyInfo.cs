using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlPropertyInfo
    {
        /// <summary>
        /// Get associated property
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Get sql column name
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Indicate if current property is a primary key
        /// </summary>
        bool IsPrimaryKey { get; }

        /// <summary>
        /// Indicate if current property is a foreign key of specified property
        /// </summary>
        /// <param name="prop">ManyToOne property</param>
        /// <returns>True if current part is foreign key, false else</returns>
        bool IsForeignKeyOf(PropertyInfo prop);

        /// <summary>
        /// Convert property value on specified entity to sql string value equivalent
        /// </summary>
        /// <param name="sqlProviderStrategy">Strategy used to converter value</param>
        /// <param name="entity">Entity on wich getting property value</param>
        /// <returns>Sql string value equivalent</returns>
        string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object entity);
    }
}