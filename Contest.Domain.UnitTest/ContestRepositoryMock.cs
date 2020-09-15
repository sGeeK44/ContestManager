using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IContest>))]
    [Export(typeof(IRepository<Domain.Games.Contest, IContest>))]
    public class ContestRepositoryMock : RepositoryBaseMock<Domain.Games.Contest, IContest>
    {
    }
}
