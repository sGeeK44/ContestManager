using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public interface IEntity4
    {
        [DataMember]
        string Prop1 { get; set; }

        [DataMember(Name = "PROP2")]
        string Prop2 { get; set; }
    }
}