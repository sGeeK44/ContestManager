using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<TeamGameStepRelationship>))]
    [Export(typeof(ISqlRepository<TeamGameStepRelationship>))]
    public class TeamGameStepRelationshipRepository : SqlRepository<TeamGameStepRelationship> { }
}
