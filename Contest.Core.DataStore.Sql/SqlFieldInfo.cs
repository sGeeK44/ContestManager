using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.Converters;
using Contest.Core.DataStore.Sql.Attributes;

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

        internal static List<SqlFieldInfo> GetPrimaryKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlPrimaryKeyAttribute))).Select(_ => CreatePrimaryKey(_, null)).ToList();
        }

        internal static List<SqlFieldInfo> GetForeignKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlForeignKeyAttribute))).Select(_ => CreateForeignKey(_, null)).ToList();
        }

        public static SqlFieldInfo Create(string name, object value, object[] customAttr)
        {
            return new SqlFieldInfo(null, name, value, customAttr);
        }

        public static List<SqlFieldInfo> GetSqlField<T>(object item = null)
        {
            var result = new List<SqlFieldInfo>();
            foreach (var prop in GetPropertiesList<T>().Where(_ => _.IsDefined(typeof (SqlFieldAttribute))))
            {
                SqlFieldInfo field;
                if (prop.IsDefined(typeof (SqlPrimaryKeyAttribute))) field = CreatePrimaryKey(prop, item);
                else if (prop.IsDefined(typeof (SqlForeignKeyAttribute))) field = CreateForeignKey(prop, item);
                else field = Create(prop, item);
                result.Add(field);
            }
            return result;
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

        private static SqlFieldInfo CreatePrimaryKey(PropertyInfo prop, object item)
        {
            var customAttr = prop.GetCustomAttributes(true);
            var columnName = GetColumnName(prop);
            var itemValue = item != null ? prop.GetValue(item, null) : null;

            return new SqlPrimaryKeyFieldInfo(prop, columnName, itemValue, customAttr);
        }

        private static SqlFieldInfo CreateForeignKey(PropertyInfo prop, object item)
        {
            var customAttr = prop.GetCustomAttributes(true);
            var columnName = GetColumnName(prop);
            var itemValue = item != null ? prop.GetValue(item, null) : null;

            return new SqlForeignKeyFieldInfo(prop, columnName, itemValue, customAttr);
        }

        private static SqlFieldInfo Create(PropertyInfo prop, object item)
        {
            var customAttr = prop.GetCustomAttributes(true);
            var columnName = GetColumnName(prop);
            var itemValue = item != null ? prop.GetValue(item, null) : null;

            return new SqlFieldInfo(prop, columnName, itemValue, customAttr);
        }

        private static string GetColumnName<T>(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException("property");

            var propertyOnRequestedType = GetPropertyOnRequestedType<T>(property);
            return GetColumnName(propertyOnRequestedType);
        }

        private static string GetColumnName(PropertyInfo prop)
        {
            var attr = GetAttribute<SqlFieldAttribute>(prop);
            return attr.Name ?? prop.Name;
        }
    }
}
