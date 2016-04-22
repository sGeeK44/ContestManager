using System;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity]
    public class BasicEntity
    {
        public static Guid Guid = new Guid("27C36EC4-1AF1-40CB-95B5-6F5655B146E4");

        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlField]
        public string Name { get; set; }

        [SqlField]
        public string ReadOnlyProperty { get { return null; } }

        public string NoSerializedField { get; set; }
    }
}
