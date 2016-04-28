using System.Reflection;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public interface ISqlFieldInfo : ISqlPropertyInfo
    {
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
        /// Get property value on specified entity to sql string value equivalent
        /// </summary>
        /// <param name="sqlProviderStrategy">Strategy used to converter value</param>
        /// <param name="entity">Entity on wich getting property value</param>
        /// <returns>Sql string value equivalent</returns>
        string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object entity);

        /// <summary>
        /// Set property value on specified entity with sql string value equivalent
        /// </summary>
        /// <param name="sqlProviderStrategy">Strategy used to converter value</param>
        /// <param name="entity">Entity on wich setting property value</param>
        /// <param name="sqlValue">Sql string value equivalent</param>
        void SetValue(ISqlProviderStrategy sqlProviderStrategy, object entity, string sqlValue);
    }
}