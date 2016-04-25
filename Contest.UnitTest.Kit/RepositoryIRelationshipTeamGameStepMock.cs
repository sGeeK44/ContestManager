using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IRelationship<ITeam, IGameStep>>))]
    public class RepositoryIRelationshipTeamGameStepMock : RepositoryBaseMock<IRelationship<ITeam, IGameStep>>
    { }
}