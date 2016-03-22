using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IMatchSetting>))]
    public class RepositoryMatchSettingMock : RepositoryBaseMock<IMatchSetting>
    { }
}