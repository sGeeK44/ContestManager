using System.Reflection;
using Contest.Core.Converters;

namespace Contest.Core.DataStore.Sql
{
    public class SqlFieldInfo : SqlPropertyInfo
    {
        private readonly object _value;
        private readonly object[] _customAttr;
        private readonly string _columnName;

        internal SqlFieldInfo(PropertyInfo referenceProperty, string columnName, object value, object[] customAttr)
            : base (referenceProperty)
        {
            _value = value;
            _customAttr = customAttr;
            _columnName = columnName;
        }

        public object Value { get { return _value; } }
        public string ColumnName { get { return _columnName; } }
        public virtual bool IsPrimaryKey { get { return false; } }

        public string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy)
        {
            return ToSqlValue(sqlProviderStrategy, _value, _customAttr);
        }

        public void SetValue(IConverter converter, object objectToSet, string sqlValue)
        {
            var innerValue = converter.Convert(PropertyInfo.PropertyType, sqlValue, _customAttr);
            SetValue(objectToSet, innerValue);
        }

        public override bool IsForeignKeyOf(PropertyInfo prop)
        {
            return false;
        }

        public static string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object value, object[] customAttr)
        {
            return sqlProviderStrategy.ToSqlValue(value, customAttr);
        }
    }
}
