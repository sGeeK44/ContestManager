using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql
{
    public class SqlForeignKeyFieldInfo : SqlFieldInfo
    {
        private readonly string _oneToManyPropertyName;

        internal SqlForeignKeyFieldInfo(PropertyInfo prop, string columnName, object itemValue, object[] customAttr)
            : base(prop, columnName, itemValue, customAttr)
        {
            var attr = (SqlForeignKeyAttribute)customAttr.Single(_ => _ is SqlForeignKeyAttribute);
            _oneToManyPropertyName = attr.OneToManyPropertyName;
        }

        public override bool IsForeignKeyOf(PropertyInfo prop)
        {
            return prop.Name == _oneToManyPropertyName;
        }
    }
}