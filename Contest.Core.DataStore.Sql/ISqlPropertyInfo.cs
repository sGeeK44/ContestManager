using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlPropertyInfo
    {
        /// <summary>
        /// Get associated property
        /// </summary>
        PropertyInfo PropertyInfo { get; }
    }
}