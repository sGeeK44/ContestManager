namespace Contest.Core.DataStore.Sql.Attributes
{
    /// <summary>
    /// Flag a properties on Dto as foreign key
    /// </summary>
    public class SqlForeignKeyAttribute : SqlFieldAttribute
    {
        public SqlForeignKeyAttribute(string oneToManyPropertyName)
        {
            OneToManyPropertyName = oneToManyPropertyName;
        }

        public string OneToManyPropertyName { get; private set; }
    }
}
