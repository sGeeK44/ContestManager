using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ENTITY_3")]
    public class Entity3
    {
        [DataMember(Name = "ID")]
        public Guid Id { get; set; }

        [DataMember(Name = "NAME")]
        public string Name { get; set; }

        public static Entity3 CreateMock()
        {
            return new Entity3 { Id = new Guid("ee47a3d1-f44b-4337-b7b0-1880e21c5c5f"), Name = "Test" };
        }
    }
}
