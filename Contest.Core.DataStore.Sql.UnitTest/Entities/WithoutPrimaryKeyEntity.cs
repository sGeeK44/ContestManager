using System;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [DataContract(Name = "ENTITY_3")]
    public class WithoutPrimaryKeyEntity
    {
        [SqlField(Name = "ID")]
        public Guid Id { get; set; }

        [SqlField(Name = "NAME")]
        public string Name { get; set; }

        public static WithoutPrimaryKeyEntity CreateMock()
        {
            return new WithoutPrimaryKeyEntity { Id = new Guid("ee47a3d1-f44b-4337-b7b0-1880e21c5c5f"), Name = "Test" };
        }
    }
}
