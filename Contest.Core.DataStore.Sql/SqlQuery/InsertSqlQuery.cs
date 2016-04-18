using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class InsertSqlQuery<T, TI> : SqlQuery<T>
    {
        private readonly TI _item;

        public InsertSqlQuery(ISqlProviderStrategy sqlProviderStrategy, TI item)
            : base(sqlProviderStrategy)
        {
            _item = item;
        }

        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        public override string ToStatement()
        {
            StringBuilder columnsName = null;
            StringBuilder columnsValue = null;
            var arg = SqlFieldInfo.GetSqlField<T>(_item);
            foreach (var sqlField in arg)
            {
                //Add field name
                if (columnsName == null) columnsName = new StringBuilder(sqlField.ColumnName);
                else columnsName.Append(", " + sqlField.ColumnName);

                // Add marker to values.
                var value = sqlField.ToSqlValue(SqlProviderStrategy);
                if (columnsValue == null) columnsValue = new StringBuilder(value);
                else columnsValue.Append(", " + value);
            }

            return string.Format(@"INSERT INTO {0} ({1}) VALUES ({2});", TableName, columnsName, columnsValue);
        }
    }
}
