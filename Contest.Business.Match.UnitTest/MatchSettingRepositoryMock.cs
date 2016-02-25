using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IMatchSetting>))]
    [Export(typeof(ISqlRepository<IMatchSetting>))]
    public class MatchSettingRepositoryMock : SqlRepositoryBaseMock<IMatchSetting>, ISqlRepository<IMatchSetting>
    {
    }
}