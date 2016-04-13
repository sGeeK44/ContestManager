using System.Linq;

namespace Contest.Core.Repository.Sql
{
    public class SqlField
    {
        public string ColumnName { get; private set; }
        public string MarkerValue { get; private set; }
        public object Value { get; private set; }
        public object[] CustomAttr { get; set; }
        public bool IsPrimaryKey { get; private set; }

        public static SqlField Create(string name, object value, object[] customAttr)
        {
            return new SqlField
            {
                ColumnName = name,
                MarkerValue = string.Concat("@", name, "@"),
                Value = value,
                CustomAttr = customAttr,
                IsPrimaryKey = customAttr != null && customAttr.OfType<SqlPrimaryKeyAttribute>().Any()
            };
        }

    }
}
