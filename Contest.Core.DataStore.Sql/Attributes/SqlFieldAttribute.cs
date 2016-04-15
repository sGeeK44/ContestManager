using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SqlFieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
