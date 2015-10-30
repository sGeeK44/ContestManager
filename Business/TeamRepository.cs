using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<ITeam>))]
    [Export(typeof(ISqlRepository<ITeam>))]
    public class TeamRepository : SqlRepository<Team, ITeam> { }
}
