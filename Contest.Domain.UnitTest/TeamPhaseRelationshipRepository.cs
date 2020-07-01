using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<Relationship<Team, Phase>, IRelationship <ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : RepositoryBaseMock<TeamPhaseRelationship, IRelationship<ITeam, IPhase>>
    {
    }
}