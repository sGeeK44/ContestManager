using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ENTITY_2")]
    public class Entity2 : IIdentifiable, IQueryable
    {
        [DataMember(Name = "ID")]
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [DataMember(Name = "NAME")]
        public string Name { get; set; }

        [DataMember(Name = "FK_ENTITY_1_NAME")]
        public Guid IdEntity1 { get; private set; }

        public Entity1 Entity { get; private set; }

        public static Entity2 CreateMock()
        {
            return new Entity2 { Id = new Guid("ee47a3d1-f43b-4337-b7b0-1880e21c5c5f"), Name = "Test", IdEntity1 = new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C") };
        }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
