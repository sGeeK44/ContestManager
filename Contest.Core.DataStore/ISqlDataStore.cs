using System;

namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Expose commons methods needed to store data into an Relation Database
    /// </summary>
    public interface ISqlDataStore
    {
        /// <summary>
        /// Convert specified .Net type into Sql column type
        /// </summary>
        /// <param name="objectType">.Net object type</param>
        /// <returns>Sql column type equivalent</returns>
        string ToSqlType(Type objectType);
    }
}