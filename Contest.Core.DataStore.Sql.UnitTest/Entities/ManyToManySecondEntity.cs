using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity]
    public class ManyToManySecondEntity : IIdentifiable, IQueryable
    {
        public static readonly Guid Guid = new Guid("071318CC-3E81-4243-803A-4B910668BC03");

        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IList<ManyToManyFirstEntity> FirstEntityList { get; private set; }

        public static ManyToManySecondEntity Create()
        {
            return new ManyToManySecondEntity
            {
                Id = Guid
            };
        }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
