using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public abstract class SqlPropertyInfo : ISqlPropertyInfo
    {
        /// <summary>
        /// Get associated property
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Indicate if current property is a foreign key of specified property
        /// </summary>
        /// <param name="prop">ManyToOne property</param>
        /// <returns>True if current part is foreign key, false else</returns>
        public abstract bool IsForeignKeyOf(PropertyInfo prop);

        protected SqlPropertyInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        protected void SetValue(object objectToSet, object innerValue)
        {
            if (!PropertyInfo.CanWrite) throw new NotSupportedException(string.Format("You have flags property as SqlField, but setter isn't accessible. Property involve: {0} ({1}).", PropertyInfo.Name, PropertyInfo.PropertyType));
            if (innerValue != null) PropertyInfo.SetValue(objectToSet, innerValue, null);
        }

        protected static List<PropertyInfo> GetPropertiesList<T>()
        {
            return GetPropertiesList(typeof(T));
        }

        protected static List<PropertyInfo> GetPropertiesList(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => item.IsDefined(typeof(SqlPropertyAttribute), true))
                    .Union(t.BaseType != null ? GetPropertiesList(t.BaseType).ToArray() : new PropertyInfo[0])
                    .ToList();
        }

        protected static PropertyInfo GetPropertyOnRequestedType<T>(PropertyInfo property)
        {
            var requestedProperty = GetPropertiesList<T>().FirstOrDefault(_ => _.Name == property.Name);

            if (requestedProperty == null) throw new NotSupportedException(string.Format("Type doesn't contains member expression property. Requested type:{0}. Property name:{1}.", typeof(T), property.Name));
            return requestedProperty;
        }

        protected static T GetAttribute<T>(PropertyInfo propertyOnRequestedType) where T : SqlPropertyAttribute
        {
            var sqlAttribute = propertyOnRequestedType.GetCustomAttributes(typeof(T), true).Cast<T>().FirstOrDefault();
            if (sqlAttribute == null) throw new NotSupportedException(string.Format("ProperType doesn't contains {0} attribute. Property:{1}", typeof(T).Name, propertyOnRequestedType.Name));

            return sqlAttribute;
        }
    }
}
