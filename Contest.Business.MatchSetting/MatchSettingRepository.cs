using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IMatchSetting>))]
    [Export(typeof(ISqlRepository<IMatchSetting>))]
    public class MatchSettingRepository : SqlRepositoryBase<MatchSetting, IMatchSetting> { }
}
