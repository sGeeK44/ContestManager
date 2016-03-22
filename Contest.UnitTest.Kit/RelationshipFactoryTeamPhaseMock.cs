using System.ComponentModel.Composition;
using Contest.Business;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRelationshipFactory<ITeam, IPhase>))]
    public class RelationshipFactoryTeamPhaseMock : RelationshipFactoryBaseMock<ITeam, IPhase> { }
}