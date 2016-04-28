using System.Linq;
using System.Reflection;
using Contest.Core.Converters;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlFieldInfo : SqlPropertyInfo
    {
        private readonly object[] _customAttr;
        private readonly string _columnName;

        internal SqlFieldInfo(PropertyInfo referenceProperty, object[] customAttr)
            : base (referenceProperty)
        {
            _customAttr = customAttr;
            _columnName = GetColumnName();
        }

        private string GetColumnName()
        {
            // Entity field can be PrimaryKey and ForeignKey at same time. in this case Column Name is carry by primary key.
            var sqlFieldAttribute = (SqlFieldAttribute)_customAttr.SingleOrDefault(_ => _ is SqlPrimaryKeyAttribute)
                                 ?? (SqlFieldAttribute)_customAttr.Single(_ => _ is SqlFieldAttribute);

            return sqlFieldAttribute.GetColumnName(PropertyInfo);
        }

        public override string ColumnName { get { return _columnName; } }

        public override bool IsPrimaryKey { get { return false; } }

        public override string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object value)
        {
            var propValue = PropertyInfo.GetValue(value);
            return sqlProviderStrategy.ToSqlValue(propValue, _customAttr);
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
    }
}
