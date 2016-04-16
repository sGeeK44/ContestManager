using System;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract]
    public class ManyToOneEntity : IIdentifiable, IQueryable
    {
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlField]
        public string Name { get; set; }

        [SqlForeignKey(typeof(OneToManyEntity))]
        public Guid OneToManyEntityId { get; private set; }

        public OneToManyEntity Entity { get; private set; }

        public static ManyToOneEntity Create()
        {
            return new ManyToOneEntity
            {
                Id = new Guid("ee47a3d1-f43b-4337-b7b0-1880e21c5c5f"),
                OneToManyEntityId = OneToManyEntity.Guid,
                Name = "Test"
            };
        }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
