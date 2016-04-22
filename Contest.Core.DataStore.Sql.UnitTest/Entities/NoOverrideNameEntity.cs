using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    [SqlEntity]
    public class NoOverrideNameEntity
    {
        [SqlField]
        public string Name { get; set; }

        public static NoOverrideNameEntity CreateMock()
        {
            return new NoOverrideNameEntity { Name = "Test" };
        }
    }
}