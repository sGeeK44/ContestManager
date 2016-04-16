using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ONE_TO_MANY")]
    public class OneToManyEntity : IQueryable
    {
        public static Guid Guid = new Guid("B495217D-C029-4295-88A9-4F6A9E8DC6A8");
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlReferenceOneToMany]
        public IEnumerable<ManyToOneEntity> EntityList { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }

        public OneToManyEntity Create()
        {
            return new OneToManyEntity { Id = Guid };
        }
    }
}