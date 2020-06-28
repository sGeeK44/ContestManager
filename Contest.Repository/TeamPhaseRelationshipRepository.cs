using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Repository
{
    [Export(typeof(SmartWay.Orm.Repositories.IRepository<Domain.IRelationship<ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : SqlRepositoryBase<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> { }
}
