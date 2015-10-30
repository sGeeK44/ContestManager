using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ENTITY_1")]
    public class Entity1 : IIdentifiable
    {
        public static Guid Guid = new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C");
        [DataMember(Name = "ID")]
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [DataMember(Name = "NAME")]
        public string Name { get; set; }

        [DataMember(Name = "ACTIVE")]
        public bool Active { get; set; }

        [DataMember(Name = "AGE")]
        public int Age { get; set; }

        public static Entity1 CreateMock()
        {
            return new Entity1 { Id = Guid, Name = "Test" };
        }

        public bool Equals(IIdentifiable other)
        {
            throw new NotImplementedException();
        }
    }
}
