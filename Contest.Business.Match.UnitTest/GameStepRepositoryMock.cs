using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IGameStep>))]
    [Export(typeof(ISqlRepository<IGameStep>))]
    public class GameStepRepositoryMock : SqlRepositoryBaseMock<IGameStep>, ISqlRepository<IGameStep>
    {
    }
}