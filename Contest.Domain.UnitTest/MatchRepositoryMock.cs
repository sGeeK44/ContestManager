using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatch>))]
    [Export(typeof(IRepository<Match, IMatch>))]
    public class MatchRepositoryMock : RepositoryBaseMock<Match, IMatch> { }
}
