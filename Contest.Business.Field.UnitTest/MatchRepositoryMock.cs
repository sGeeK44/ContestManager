using System.ComponentModel.Composition;
using Contest.Business.UnitTest;
using Contest.Core.Repository;

namespace Contest.Business.Fields.UnitTest
{
    [Export(typeof(IRepository<IMatch>))]
    public class MatchRepositoryMock : RepositoryBaseMock<IMatch>, IRepository<IMatch>
    {
    }
}
