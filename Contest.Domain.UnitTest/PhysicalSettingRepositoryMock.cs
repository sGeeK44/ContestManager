using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IPhysicalSetting>))]
    public class PhysicalSettingRepositoryMock : RepositoryBaseMock<IPhysicalSetting>
    {
    }
}