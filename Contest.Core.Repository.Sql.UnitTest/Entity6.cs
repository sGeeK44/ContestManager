using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract]
    public class Entity6
    {
        [DataMember]
        public string Name { get; set; }

        public static Entity6 CreateMock()
        {
            return new Entity6 { Name = "Test" };
        }
    }
}
