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
        /// Close active connection with database
        /// </summary>
        void CloseDatabase();

        /// <summary>
        /// Execute specified request
        /// </summary>
        /// <param name="request">Sql request to execute</param>
        /// <return>Sql result</return>
        IDataReader Execute(ISqlQuery request);

        /// <summary>
        /// Cancel all request added after last commit
        /// </summary>
        void RollBack();

        /// <summary>
        /// Execute all request add after previous commit
        /// </summary>
        void Commit();

        /// <summary>
        /// Add new request for next trasaction
        /// </summary>
        /// <param name="request">Request to apped</param>
        void AddRequest(ISqlQuery request);
    }
}