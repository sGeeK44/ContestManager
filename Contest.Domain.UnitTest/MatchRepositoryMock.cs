using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<Match, IMatch>))]
    public class MatchRepositoryMock : RepositoryBaseMock<Match, IMatch> { }
}
