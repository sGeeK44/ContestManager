using System;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    public class OneToManyWithoutIListEntity
    {
        public static Guid Guid = new Guid("F3AF2B6E-3C41-44F3-BAD4-B9D598CE5C5B");
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlOneToManyReference]
        public object EntityList { get; set; }

        public static OneToManyWithoutIListEntity Create()
        {
            return new OneToManyWithoutIListEntity { Id = Guid };
        }
    }
}