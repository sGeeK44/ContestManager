using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPhase>>))]
    [Export(typeof(ISqlRepository<IRelationship<ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : SqlRepository<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> { }
}
