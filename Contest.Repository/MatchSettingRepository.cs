using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IMatchSetting>))]
    [Export(typeof(IRepository<MatchSetting, IMatchSetting>))]
    public class MatchSettingRepository : SqlRepositoryBase<MatchSetting, IMatchSetting> { }
}
