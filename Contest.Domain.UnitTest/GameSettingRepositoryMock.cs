using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IGameSetting>))]
    public class GameSettingRepositoryMock : RepositoryBaseMock<IGameSetting>
    {
    }
}