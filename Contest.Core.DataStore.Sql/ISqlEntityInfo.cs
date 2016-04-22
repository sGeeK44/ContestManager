using System;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlEntityInfo
    {
        /// <summary>
        /// Get sql table name of entity
        /// </summary>
        string TableName { get; }
    }
}