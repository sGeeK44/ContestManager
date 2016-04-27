using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity(Name = "ONE_TO_MANY")]
    public class OneToManyEntity
    {
        public static Guid Guid = new Guid("B495217D-C029-4295-88A9-4F6A9E8DC6A8");
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlOneToManyReference]
        public IList<ManyToOneEntity> EntityList { get; set; }

        public static OneToManyEntity Create()
        {
            return new OneToManyEntity { Id = Guid };
        }
    }
}