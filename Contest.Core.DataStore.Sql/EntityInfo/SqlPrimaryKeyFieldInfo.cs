using System.Reflection;

namespace Contest.Core.DataStore.Sql
{
    public class SqlPrimaryKeyFieldInfo : SqlFieldInfo
    {
        internal SqlPrimaryKeyFieldInfo(PropertyInfo prop, string columnName, object itemValue, object[] customAttr)
            : base(prop, columnName, itemValue, customAttr) { }

        public override bool IsPrimaryKey { get { return true; } }
    }
}