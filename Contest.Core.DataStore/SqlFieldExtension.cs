using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql
{
    public static class SqlFieldExtension
    {
        public static string ColumnName<TObj, TPropOrField>(this TObj obj, Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            return ColumnName(propertyorFieldExpression);
        }

        public static string ColumnName<TObj, TPropOrField>(Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            if (propertyorFieldExpression == null) throw new ArgumentNullException("propertyorFieldExpression");

            var body = propertyorFieldExpression.Body as MemberExpression;

            return ColumnName<TObj>(body);
        }

        public static string ColumnName<T>(this MemberExpression memberExpression)
        {
            if (memberExpression == null) throw new ArgumentNullException("memberExpression");

            return ColumnName<T>(memberExpression.Member as PropertyInfo);
        }

        private static string ColumnName<T>(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var propertyOnRequestedType = GetPropertyOnRequestedType<T>(property);
            var sqlAttribute = GetDataMemberAttribute(propertyOnRequestedType);

            return ColumnName(sqlAttribute, propertyOnRequestedType);
        }

        private static DataMemberAttribute GetDataMemberAttribute(PropertyInfo propertyOnRequestedType)
        {
            var sqlAttribute = propertyOnRequestedType.GetCustomAttributes(typeof(DataMemberAttribute), true).Cast<DataMemberAttribute>().First();
            
            if (sqlAttribute == null) throw new NotSupportedException("Properties doesn't contains DataMemberAttribute.");
            return sqlAttribute;
        }

        private static PropertyInfo GetPropertyOnRequestedType<T>(PropertyInfo property)
        {
            var requestedProperty = GetPropertiesList<T>().FirstOrDefault(_ => _.Name == property.Name);

            if (requestedProperty == null) throw new NotSupportedException(string.Format("Type doesn't contains member expression property. Requested type:{0}. Property name:{1}.", typeof(T), property.Name));
            return requestedProperty;
        }

        private static string ColumnName(DataMemberAttribute attr, PropertyInfo prop)
        {
            return attr.Name ?? prop.Name;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesList<T>()
        {
            return GetPropertiesList(typeof(T));
        }

        public static IEnumerable<PropertyInfo> GetPropertiesList(this Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => item.IsDefined(typeof(DataMemberAttribute), true))
                    .Union(t.BaseType != null ? t.BaseType.GetPropertiesList() : new PropertyInfo[0]);
        }

        public static List<SqlField> GetSqlField<T>(object item)
        {
            var propList = GetPropertiesList<T>();

            var result = new List<SqlField>();
            foreach (var prop in propList)
            {
                var customAttr = prop.GetCustomAttributes(true);
                var fieldAttribute = customAttr.OfType<DataMemberAttribute>().FirstOrDefault();
                if (fieldAttribute == null) continue;

                var columnName = ColumnName(fieldAttribute, prop);
                var itemValue = item != null ? prop.GetValue(item, null) : null;
                result.Add(SqlField.Create(columnName, itemValue, customAttr));
            }

            return result;
        }
    }
}
