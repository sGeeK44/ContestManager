using System.ComponentModel.Composition;
using Contest.Business.UnitTest;
using Contest.Core.Repository;

namespace Contest.Business.Fields.UnitTest
{
    [Export(typeof(IRepository<IContest>))]
    public class ContestRepositoryMock : RepositoryBaseMock<IContest>, IRepository<IContest>
    {
    }
}
