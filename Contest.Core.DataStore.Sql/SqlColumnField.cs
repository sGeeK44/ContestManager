using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public object Value { get { return _value; } }
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

        public static List<SqlColumnField> GetSqlField<T>(object item)
        {
            var propList = GetPropertiesList<T>();

            var result = new List<SqlColumnField>();
            foreach (var prop in propList)
            {
                var customAttr = prop.GetCustomAttributes(true);
                var fieldAttribute = customAttr.OfType<SqlFieldAttribute>().FirstOrDefault();
                if (fieldAttribute == null) continue;

                var columnName = GetColumnName(fieldAttribute, prop);
                var itemValue = item != null ? prop.GetValue(item, null) : null;
                result.Add(Create(columnName, itemValue, customAttr));
            }

            return result;
        }

        public static string GetColumnName<TObj, TPropOrField>(TObj obj, Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            return GetColumnName(propertyorFieldExpression);
        }

        public static string GetColumnName<TObj, TPropOrField>(Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            if (propertyorFieldExpression == null) throw new ArgumentNullException("propertyorFieldExpression");

            var body = propertyorFieldExpression.Body as MemberExpression;

            return GetColumnName<TObj>(body);
        }

        public static string GetColumnName<T>(MemberExpression memberExpression)
        {
            if (memberExpression == null) throw new ArgumentNullException("memberExpression");

            return GetColumnName<T>(memberExpression.Member as PropertyInfo);
        }

        public static string GetColumnName<T>(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var propertyOnRequestedType = GetPropertyOnRequestedType<T>(property);
            var sqlAttribute = GetSqlFieldAttribute(propertyOnRequestedType);

            return GetColumnName(sqlAttribute, propertyOnRequestedType);
        }

        private static SqlFieldAttribute GetSqlFieldAttribute(PropertyInfo propertyOnRequestedType)
        {
            var sqlAttribute = propertyOnRequestedType.GetCustomAttributes(typeof(SqlFieldAttribute), true).Cast<SqlFieldAttribute>().FirstOrDefault();
            if (sqlAttribute == null) throw new NotSupportedException(string.Format("Properties doesn't contains SqlFieldAttribute. Property:{0}", propertyOnRequestedType.Name));

            return sqlAttribute;
        }

        private static PropertyInfo GetPropertyOnRequestedType<T>(PropertyInfo property)
        {
            var requestedProperty = GetPropertiesList<T>().FirstOrDefault(_ => _.Name == property.Name);

            if (requestedProperty == null) throw new NotSupportedException(string.Format("Type doesn't contains member expression property. Requested type:{0}. Property name:{1}.", typeof(T), property.Name));
            return requestedProperty;
        }

        private static string GetColumnName(SqlFieldAttribute attr, PropertyInfo prop)
        {
            return attr.Name ?? prop.Name;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesList<T>()
        {
            return GetPropertiesList(typeof(T));
        }

        public static IEnumerable<PropertyInfo> GetPropertiesList(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => item.IsDefined(typeof(SqlFieldAttribute), true))
                    .Union(t.BaseType != null ? GetPropertiesList(t.BaseType) : new PropertyInfo[0]);
        }
    }
}
