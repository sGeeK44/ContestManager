using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IMatch>))]
    public class MatchRepository : SqlRepositoryBase<Match, IMatch> { }
}
