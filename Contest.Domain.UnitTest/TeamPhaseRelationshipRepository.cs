using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : RepositoryBaseMock<IRelationship<ITeam, IPhase>>
    {
    }
}