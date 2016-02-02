using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract]
    public class Entity7
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NameLong { get; set; }

        public static Entity7 CreateMock()
        {
            return new Entity7 { Name = "Value1", NameLong = "Value2" };
        }
    }
}
