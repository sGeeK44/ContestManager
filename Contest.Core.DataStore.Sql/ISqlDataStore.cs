using System.Collections.Generic;
using System.Data;
using Contest.Core.DataStore.Sql.SqlQuery;

namespace Contest.Core.DataStore.Sql
{
    /// <summary>
    /// Expose commons methods needed to store data into an Relation Database
    /// </summary>
    public interface ISqlDataStore
    {
        /// <summary>
        /// Create new active connection with database
        /// </summary>
        void OpenDatabase();

        /// <summary>
        /// Execute specified request
        /// </summary>
        /// <param name="request">Sql request to execute</param>
        /// <return>Sql result</return>
        IDataReader Execute(ISqlQuery request);

        /// <summary>
        /// Close active connection with database
        /// </summary>
        void CloseDatabase();
        void RollBack();
        void Commit();
        void AddRequest(ISqlQuery request);
    }
}