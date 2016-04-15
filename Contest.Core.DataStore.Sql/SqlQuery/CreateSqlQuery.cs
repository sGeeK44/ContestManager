using System;
using System.Linq;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class CreateSqlQuery<T> : SqlQuery<T>
    {
        public CreateSqlQuery(ISqlProviderStrategy sqlProviderStrategy) : base(sqlProviderStrategy) { }

        public override string ToStatement()
        {
            var fieldList = (from propertyInfo in SqlColumnField.GetSqlField<T>()
                             select string.Format("{0} {1}{2}",
                                                 propertyInfo.ColumnName,
                                                 SqlProviderStrategy.ToSqlType(propertyInfo.Property.PropertyType),
                                                 propertyInfo.IsPrimaryKey ? " primary key" : string.Empty)).ToList();


            if (fieldList.Count == 0) throw new NotSupportedException("Table have to own one field at less.");
            
            return string.Format(@"CREATE TABLE IF NOT EXISTS {0} ({1});", TableName, fieldList.Aggregate((x, y) => string.Concat(x, ", ", y)));
        }
    }
}
