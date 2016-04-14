using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlColumnField
    {
        private readonly object _value;
        private readonly object[] _customAttr;

        private SqlColumnField(object value, object[] customAttr)
        {
            _value = value;
            _customAttr = customAttr;
        }

        public string ColumnName { get; private set; }
        public string MarkerValue { get; private set; }
        public bool IsPrimaryKey { get; private set; }

        public string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy)
        {
            return sqlProviderStrategy.ToSqlValue(_value, _customAttr);
        }

        public static SqlColumnField Create(string name, object value, object[] customAttr)
        {
            return new SqlColumnField(value, customAttr)
            {
                ColumnName = name,
                MarkerValue = string.Concat("@", name, "@"),
                IsPrimaryKey = customAttr != null && customAttr.OfType<SqlPrimaryKeyAttribute>().Any()
            };
        }
    }
}
