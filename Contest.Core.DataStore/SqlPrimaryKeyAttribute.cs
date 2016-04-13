using System;

namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Flag a properties on Dto as primary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SqlPrimaryKeyAttribute : Attribute { }
}
