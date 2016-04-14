using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    /// <summary>
    /// Flag a properties on Dto as primary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SqlPrimaryKeyAttribute : SqlFieldAttribute { }
}
