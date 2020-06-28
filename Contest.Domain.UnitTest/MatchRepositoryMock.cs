using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatch>))]
    public class MatchRepositoryMock : RepositoryBaseMock<IMatch> { }
}
