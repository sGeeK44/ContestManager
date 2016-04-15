using Contest.Core.Serialization;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    public enum EnumPivot
    {
        [AssociatedClass(typeof(BasicEntity))]
        BasicAssociatedClass,

        [AssociatedClass(typeof(DynamicEntity))]
        DynamicAssociatedClass,

        [AssociatedClass(typeof(GenericEntity<>))]
        GenericAssociatedClass,
    }
}