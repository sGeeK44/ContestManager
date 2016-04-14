using System;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class CreateSqlQuery<T> : SqlQuery<T>
    {
        public CreateSqlQuery(ISqlProviderStrategy sqlProviderStrategy) : base(sqlProviderStrategy) { }

        public override string ToStatement()
        {
            var fieldList = (from propertyInfo in SqlColumnField.GetPropertiesList<T>()
                             let fieldAttribute = propertyInfo.GetCustomAttributes(typeof(SqlFieldAttribute), true)
                                                             .Cast<SqlFieldAttribute>()
                                                             .FirstOrDefault()
                             where fieldAttribute != null
                             select string.Format("{0} {1}{2}",
                                                 fieldAttribute.Name ?? propertyInfo.Name,
                                                 SqlProviderStrategy.ToSqlType(propertyInfo.PropertyType),
                                                 propertyInfo.GetCustomAttributes(typeof(SqlPrimaryKeyAttribute), true)
                                                             .Cast<SqlPrimaryKeyAttribute>()
                                                             .FirstOrDefault() != null ? " primary key" : string.Empty)).ToList();


            if (fieldList.Count == 0) throw new NotSupportedException("Table have to own one field at less.");
            
            return string.Format(@"CREATE TABLE IF NOT EXISTS {0} ({1});", TableName, fieldList.Aggregate((x, y) => string.Concat(x, ", ", y)));
        }
    }
}
