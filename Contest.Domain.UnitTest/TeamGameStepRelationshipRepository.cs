using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>>))]
    public class TeamGameStepRelationshipRepository : RepositoryBaseMock<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>>
    {
    }
}