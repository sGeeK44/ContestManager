using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlFieldInfo : SqlPropertyInfo, ISqlFieldInfo
    {
        private readonly object[] _customAttr;
        private readonly string _columnName;

        internal SqlFieldInfo(PropertyInfo referenceProperty)
            : base (referenceProperty)
        {
            _customAttr = referenceProperty.GetCustomAttributes(true);
            _columnName = GetColumnName();
        }

        private string GetColumnName()
        {
            // Entity field can be PrimaryKey and ForeignKey at same time. in this case Column Name is carry by primary key.
            var sqlFieldAttribute = (SqlFieldAttribute)_customAttr.SingleOrDefault(_ => _ is SqlPrimaryKeyAttribute)
                                 ?? (SqlFieldAttribute)_customAttr.Single(_ => _ is SqlFieldAttribute);

            return sqlFieldAttribute.GetColumnName(PropertyInfo);
        }

        protected object[] CustomAttr { get { return _customAttr; } }

        public virtual string ColumnName { get { return _columnName; } }

        public virtual bool IsPrimaryKey { get { return false; } }

        public virtual bool IsForeignKeyOf(PropertyInfo prop)
        {
            return false;
        }

        public string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object value)
        {
            var propValue = PropertyInfo.GetValue(value);
            return sqlProviderStrategy.ToSqlValue(propValue, _customAttr);
        }

        public void SetValue(ISqlProviderStrategy strategy, object objectToSet, string sqlValue)
        {
            var innerValue = strategy.FromSqlValue(PropertyInfo.PropertyType, sqlValue, _customAttr);
            SetValue(objectToSet, innerValue);
        }
    }
}
