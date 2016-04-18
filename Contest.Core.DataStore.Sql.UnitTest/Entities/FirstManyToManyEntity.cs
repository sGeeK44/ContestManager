using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [DataContract]
    public class FirstManyToManyEntity : IIdentifiable
    {
        public static Guid Guid = new Guid("5C8C1158-E011-41C8-BF35-9CCF3A734396");
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IEnumerable<SecondManyToManyEntity> EntityList { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}