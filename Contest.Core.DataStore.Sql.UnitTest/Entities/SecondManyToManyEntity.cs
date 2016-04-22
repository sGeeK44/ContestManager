using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity]
    public class SecondManyToManyEntity : IIdentifiable
    {
        public static Guid Guid = new Guid("937E83C6-61C4-41E2-BD99-4B6A99E1B7FC");
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IEnumerable<FirstManyToManyEntity> EntityList { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}