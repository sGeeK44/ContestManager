using Contest.Core.Serialization;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [GenericClass(typeof(EnumPivot))]
    public class GenericEntity<T> : EnumPivotEntity { }
}
