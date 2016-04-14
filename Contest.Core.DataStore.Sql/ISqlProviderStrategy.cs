using System;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlProviderStrategy
    {
        /// <summary>
        /// Convert specified .Net type into Sql column type
        /// </summary>
        /// <param name="objectType">.Net object type</param>
        /// <returns>Sql column type equivalent</returns>
        string ToSqlType(Type objectType);

        /// <summary>
        /// Convert specified object to string value
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="attr">Custom attribute attach to property which hold obj value</param>
        /// <returns>Sql string value representation</returns>
        string ToSqlValue(object obj, object[] attr);
    }
}
