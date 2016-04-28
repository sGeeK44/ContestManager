using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class InsertSqlQuery<T, TI> : SqlQuery<T>
    {
        private readonly TI _item;

        public InsertSqlQuery(ISqlProviderStrategy sqlProviderStrategy, IEntityInfoFactory entityInfoFactory, TI item)
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
            StringBuilder columnsName = null;
            StringBuilder columnsValue = null;
            var entity = EntityInfoFactory.GetEntityInfo<T>();
            foreach (var sqlField in entity.FieldList)
            {
                //Add field name
                if (columnsName == null) columnsName = new StringBuilder(sqlField.ColumnName);
                else columnsName.Append(", " + sqlField.ColumnName);

                // Add marker to values.
                var value = sqlField.ToSqlValue(SqlProviderStrategy, _item);
                if (columnsValue == null) columnsValue = new StringBuilder(value);
                else columnsValue.Append(", " + value);
            }

            return string.Format(@"INSERT INTO {0} ({1}) VALUES ({2});", TableName, columnsName, columnsValue);
        }
    }
}
