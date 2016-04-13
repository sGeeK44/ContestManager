using System;
using System.Collections.Generic;
using System.Data;

namespace Contest.Core.DataStore
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

        void OpenDatabase();

        void Execute(IList<string> requestList);

        void CloseDatabase();

        IDataReader Execute(string request);
    }
}