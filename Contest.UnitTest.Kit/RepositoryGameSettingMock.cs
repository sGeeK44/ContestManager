using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IGameSetting>))]
    public class RepositoryGameSettingMock : RepositoryBaseMock<IGameSetting>
    { }
}