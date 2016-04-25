using System.ComponentModel.Composition;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPhase>>))]
    [Export(typeof(ISqlRepository<IRelationship<ITeam, IPhase>>))]
    public class TeamPhaseRelationshipRepository : SqlRepositoryBase<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> { }
}
