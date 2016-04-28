using System;
using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class UpdateSqlQuery<T, TI> : SqlQuery<T>
    {
        private readonly TI _item;

        public UpdateSqlQuery(ISqlProviderStrategy sqlProviderStrategy, IEntityInfoFactory entityInfoFactory, TI item)
            : base(sqlProviderStrategy, entityInfoFactory)
        {
            _item = item;
        }

        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        public override string ToStatement()
        {
            StringBuilder values = null;
            StringBuilder keys = null;
            var entityInfo = EntityInfoFactory.GetEntityInfo<T>();
            foreach (var sqlField in entityInfo.FieldList)
            {
                var value = sqlField.ToSqlValue(SqlProviderStrategy, _item);
                if (sqlField.IsPrimaryKey)
                {
                    // Add field to where clause.
                    if (keys == null) keys = new StringBuilder(sqlField.ColumnName + " = " + value);
                    else keys.Append(" AND " + sqlField.ColumnName + " = " + value);
                }
                else
                {
                    //Add marker to set clause
                    if (values == null) values = new StringBuilder(sqlField.ColumnName + " = " + value);
                    else values.Append(", " + sqlField.ColumnName + " = " + value);
                }
            }
            if (keys == null) throw new NotSupportedException(string.Format("Can not update class without primary key. Class:{0}", _item.GetType().Name));
            return string.Format(@"UPDATE {0} SET {1} WHERE {2};", TableName, values, keys);
        }
    }
}
