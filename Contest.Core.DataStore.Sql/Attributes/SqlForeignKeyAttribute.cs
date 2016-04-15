using System;

namespace Contest.Core.DataStore.Sql.Attributes
{
    /// <summary>
    /// Flag a properties on Dto as foreign key
    /// </summary>
    public class SqlForeignKeyAttribute : SqlFieldAttribute
    {
        public SqlForeignKeyAttribute(Type foreignEntity)
        {

        }

        public string PrimaryKeyPropertyName { get; set; }
    }
}
