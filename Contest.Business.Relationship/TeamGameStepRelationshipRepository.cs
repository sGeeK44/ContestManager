using System.ComponentModel.Composition;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IRelationship<ITeam, IGameStep>>))]
    [Export(typeof(ISqlRepository<IRelationship<ITeam, IGameStep>>))]
    public class TeamGameStepRelationshipRepository : SqlRepositoryBase<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>> { }
}
