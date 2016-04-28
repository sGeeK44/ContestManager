using System.Reflection;

namespace Contest.Core.DataStore.Sql.Attributes
{
    public class SqlFieldAttribute : SqlPropertyAttribute
    {
        public string Name { get; set; }

        internal string GetColumnName(PropertyInfo prop)
        {
            return Name ?? prop.Name;
        }
    }
}
