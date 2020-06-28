using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IMatchSetting>))]
    public class MatchSettingRepository : SqlRepositoryBase<MatchSetting, IMatchSetting> { }
}
