using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    public class EnumPivotEntity
    {
        [SqlField]
        public EnumPivot Pivot { get; set; }
    }
}
