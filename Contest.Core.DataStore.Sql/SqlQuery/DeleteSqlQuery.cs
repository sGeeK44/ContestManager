using System;
using System.Linq;
using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class DeleteSqlQuery<T, TI> : SqlQuery<T>
    {
        private readonly TI _item;

        public DeleteSqlQuery(ISqlProviderStrategy sqlProviderStrategy, TI item) : base(sqlProviderStrategy)
        {
            _item = item;
        }

        public override string ToStatement()
        {
            var arg = EntityInfoFactory.GetSqlField<T>(_item).Where(_ => _.IsPrimaryKey).ToList();
            StringBuilder keys = null;
            //For each primary keys
            foreach (var sqlField in arg)
            {
                var value = sqlField.ToSqlValue(SqlProviderStrategy);
                // Add field to where clause.
                if (keys == null) keys = new StringBuilder(sqlField.ColumnName + " = " + value);
                else keys.Append(" AND " + sqlField.ColumnName + " = " + value);
            }
            if (keys == null) throw new NotSupportedException(string.Format("Can not delete class without primary key. Class:{0}", _item.GetType().Name));

            return string.Format(@"DELETE FROM {0} WHERE {1};", TableName, keys);
        }
    }
}
