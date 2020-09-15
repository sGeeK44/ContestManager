using System.ComponentModel.Composition;
using Contest.Business.UnitTest;
using Contest.Domain.Games;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Domain.UnitTest
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPhase>>))]
    [Export(typeof(IRepository<TeamPhaseRelationship, IRelationship <ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : RepositoryBaseMock<TeamPhaseRelationship, IRelationship<ITeam, IPhase>>
    {
    }
}