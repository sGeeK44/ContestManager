using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class RelationshipFactoryTeamPhaseMock : RelationshipFactoryBaseMock<ITeam, IPhase> { }
}