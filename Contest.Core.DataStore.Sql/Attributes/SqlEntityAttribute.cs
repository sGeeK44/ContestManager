using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    public class SqlEntityAttribute : SqlAttribute
    {
        public string Name { get; set; }

        internal virtual string GetTableName(Type classInfo)
        {
            return Name ?? classInfo.Name;
        }
    }
}
