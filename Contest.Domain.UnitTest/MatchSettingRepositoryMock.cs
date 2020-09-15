using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatchSetting>))]
    [Export(typeof(IRepository<MatchSetting, IMatchSetting>))]
    public class MatchSettingRepositoryMock : RepositoryBaseMock<MatchSetting, IMatchSetting>
    {
    }
}