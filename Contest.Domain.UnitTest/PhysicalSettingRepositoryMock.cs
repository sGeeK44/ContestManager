using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IPhysicalSetting>))]
    [Export(typeof(IRepository<PhysicalSetting, IPhysicalSetting>))]
    public class PhysicalSettingRepositoryMock : RepositoryBaseMock<PhysicalSetting, IPhysicalSetting>
    {
    }
}