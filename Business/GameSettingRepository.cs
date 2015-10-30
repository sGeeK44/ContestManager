using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IGameSetting>))]
    [Export(typeof(ISqlRepository<IGameSetting>))]
    public class GameSettingRepository : SqlRepository<GameSetting, IGameSetting> { }
}
