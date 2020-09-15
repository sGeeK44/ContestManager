using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IMatch>))]
    [Export(typeof(IRepository<Match, IMatch>))]
    public class MatchRepository : SqlRepositoryBase<Match, IMatch> { }
}
