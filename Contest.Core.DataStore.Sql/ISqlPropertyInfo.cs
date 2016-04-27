using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlPropertyInfo
    {
        /// <summary>
        /// Get associated property
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Indicate if current property is a foreign key of specified property
        /// </summary>
        /// <param name="prop">ManyToOne property</param>
        /// <returns>True if current part is foreign key, false else</returns>
        bool IsForeignKeyOf(PropertyInfo prop);
    }
}