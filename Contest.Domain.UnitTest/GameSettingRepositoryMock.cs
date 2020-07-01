using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<GameSetting, IGameSetting>))]
    public class GameSettingRepositoryMock : RepositoryBaseMock<GameSetting, IGameSetting>
    {
    }
}