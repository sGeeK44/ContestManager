using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IMatch>))]
    public class RepositoryMatchMock : RepositoryBaseMock<IMatch>
    { }
}