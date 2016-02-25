using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IGameStep>))]
    [Export(typeof(ISqlRepository<IGameStep>))]
    public class GameStepMock : RepositoryMockBase<IGameStep>, ISqlRepository<IGameStep>
    {
    }
}