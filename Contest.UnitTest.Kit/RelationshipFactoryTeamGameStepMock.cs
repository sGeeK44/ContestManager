using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRelationshipFactory<ITeam, IGameStep>))]
    public class RelationshipFactoryTeamGameStepMock : RelationshipFactoryBaseMock<ITeam, IGameStep> { }
}