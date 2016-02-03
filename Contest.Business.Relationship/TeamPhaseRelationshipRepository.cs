using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<TeamPhaseRelationship>))]
    [Export(typeof(ISqlRepository<TeamPhaseRelationship>))]
    public class TeamPhaseRelationshipRepository : SqlRepository<TeamPhaseRelationship, TeamPhaseRelationship> { }
}
