using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Repository
{
    [Export(typeof(IRepository<TeamPhaseRelationship, IRelationship<ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : SqlRepositoryBase<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> { }
}
