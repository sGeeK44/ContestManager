using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class ManyToManySecondEntity : IIdentifiable, IQueryable
    {
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IList<Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>> FirstEntityList { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
