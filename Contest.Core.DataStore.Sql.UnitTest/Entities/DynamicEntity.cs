using Contest.Core.Serialization;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [DynamicClass(typeof(EnumPivot))]
    public class DynamicEntity : EnumPivotEntity { }
}
