using System.Collections.Generic;

namespace Contest.Core.DataStore.Sql
{
    public interface ISqlEntityInfo
    {
        /// <summary>
        /// Get sql table name of entity
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Get list of sql properies
        /// </summary>
        IList<ISqlPropertyInfo> FieldList { get; }
    }
}