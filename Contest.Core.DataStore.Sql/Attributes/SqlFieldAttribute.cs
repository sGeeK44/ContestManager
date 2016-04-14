using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    public class SqlFieldAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
