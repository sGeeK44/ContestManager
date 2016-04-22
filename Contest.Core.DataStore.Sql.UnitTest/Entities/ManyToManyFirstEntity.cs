using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity]
    public class ManyToManyFirstEntity : IIdentifiable, IQueryable
    {
        internal static readonly Guid Guid = new Guid("DDA6148D-7316-4C91-827B-10F6A388810E");

        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IList<ManyToManySecondEntity> SecondEntityList { get; private set; }

        public static ManyToManyFirstEntity Create()
        {
            return new ManyToManyFirstEntity
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
