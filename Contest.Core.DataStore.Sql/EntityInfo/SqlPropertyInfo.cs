using System;
using System.Reflection;

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
    }
}
