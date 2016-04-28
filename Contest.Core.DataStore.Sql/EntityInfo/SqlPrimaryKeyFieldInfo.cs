using System.Reflection;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlPrimaryKeyFieldInfo : SqlFieldInfo
    {
        internal SqlPrimaryKeyFieldInfo(PropertyInfo prop)
            : base(prop) { }

        public override bool IsPrimaryKey { get { return true; } }
    }
}