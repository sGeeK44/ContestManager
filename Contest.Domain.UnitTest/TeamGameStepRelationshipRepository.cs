using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IRelationship<ITeam, IGameStep>>))]
    public class TeamGameStepRelationshipRepository : RepositoryBaseMock<IRelationship<ITeam, IGameStep>>
    {
    }
}