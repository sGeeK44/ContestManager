using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<ITeam>))]
    [Export(typeof(IRepository<Team, ITeam>))]
    public class TeamRepository : SqlRepositoryBase<Team, ITeam> { }
}
