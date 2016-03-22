using System.ComponentModel.Composition;
using Contest.Business;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class RelationshipFactoryTeamGameStepMock : RelationshipFactoryBaseMock<ITeam, IGameStep> { }
}