using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [DataContract(Name = "ENTITY_4")]
    public class Entity4 : IEntity4
    {
        [DataMember]
        public string Prop1 { get; set; }
        [DataMember(Name = "PROP2")]
        public string Prop2 { get; set; }
    }
}
