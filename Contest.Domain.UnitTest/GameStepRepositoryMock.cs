using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IGameStep>))]
    [Export(typeof(IRepository<GameStep, IGameStep>))]
    public class GameStepRepositoryMock : RepositoryBaseMock<GameStep, IGameStep>
    {
    }
}