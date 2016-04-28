using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlForeignKeyFieldInfo : SqlFieldInfo
    {
        private readonly string _oneToManyPropertyName;

        internal SqlForeignKeyFieldInfo(PropertyInfo prop)
            : base(prop)
        {
            var foreignKey = GetSqlForeignKeyAttribute();
            _oneToManyPropertyName = foreignKey.OneToManyPropertyName;
        }

        public override bool IsForeignKeyOf(PropertyInfo prop)
        {
            return prop.Name == _oneToManyPropertyName;
        }

        private SqlForeignKeyAttribute GetSqlForeignKeyAttribute()
        {
            return (SqlForeignKeyAttribute)CustomAttr.Single(_ => _ is SqlForeignKeyAttribute);
        }
    }
}