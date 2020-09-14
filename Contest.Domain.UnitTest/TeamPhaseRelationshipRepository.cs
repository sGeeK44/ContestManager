using System.ComponentModel.Composition;
using Contest.Business.UnitTest;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Domain.UnitTest
{
    [Export(typeof(IRepository<TeamPhaseRelationship, IRelationship <ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : RepositoryBaseMock<TeamPhaseRelationship, IRelationship<ITeam, IPhase>>
    {
    }
}