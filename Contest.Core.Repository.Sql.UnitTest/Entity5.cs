using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ENTITY_5")]
    public class Entity5 : IEntity5, IQueryable
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

        public static Entity5 CreateMock()
        {
            return new Entity5 { Id = Guid, Name = "Test" };
        }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
