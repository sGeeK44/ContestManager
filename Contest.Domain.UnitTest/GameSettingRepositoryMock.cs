using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IGameSetting>))]
    [Export(typeof(IRepository<GameSetting, IGameSetting>))]
    public class GameSettingRepositoryMock : RepositoryBaseMock<GameSetting, IGameSetting>
    {
    }
}