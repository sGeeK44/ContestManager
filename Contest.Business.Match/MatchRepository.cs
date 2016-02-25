using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IMatch>))]
    [Export(typeof(ISqlRepository<IMatch>))]
    public class MatchRepository : SqlRepository<Match, IMatch> { }
}
