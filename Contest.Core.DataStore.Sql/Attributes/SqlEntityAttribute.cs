using System;
using Contest.Core.DataStore.Sql.EntityInfo;
using Contest.Core.DataStore.Sql.SqlQuery;

namespace Contest.Core.DataStore.Sql.Attributes
{
    public class SqlEntityAttribute : SqlAttribute
    {
        public string Name { get; set; }

        internal virtual string GetTableName(IEntityInfoFactory entityInfoFactory, Type classInfo)
        {
            return Name ?? classInfo.Name;
        }
    }
}
