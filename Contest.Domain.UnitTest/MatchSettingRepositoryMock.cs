using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Matchs;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<MatchSetting, IMatchSetting>))]
    public class MatchSettingRepositoryMock : RepositoryBaseMock<MatchSetting, IMatchSetting>
    {
    }
}