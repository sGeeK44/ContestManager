using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public class SqlPrimaryKeyFieldInfo : SqlFieldInfo
    {
        internal SqlPrimaryKeyFieldInfo(PropertyInfo prop, object[] customAttr)
            : base(prop, customAttr) { }

        public override bool IsPrimaryKey { get { return true; } }
    }
}