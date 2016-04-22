using System;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity(Name = "ENTITY_1")]
    public class OverrideNameEntity : IOverrideNameEntity
    {
        public static Guid Guid = new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C");

        [SqlPrimaryKey(Name = "ID")]
        public Guid Id { get; set; }

        [SqlField(Name = "NAME")]
        public string Name { get; set; }

        [SqlField(Name = "ACTIVE")]
        public bool Active { get; set; }

        [SqlField(Name = "AGE")]
        public int Age { get; set; }

        public static OverrideNameEntity CreateMock()
        {
            return new OverrideNameEntity { Id = Guid, Name = "Test" };
        }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}
