using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<PhysicalSetting, IPhysicalSetting>))]
    public class PhysicalSettingRepositoryMock : RepositoryBaseMock<PhysicalSetting, IPhysicalSetting>
    {
    }
}