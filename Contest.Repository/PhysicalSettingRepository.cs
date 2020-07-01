using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Repository
{
    [Export(typeof(IRepository<PhysicalSetting, IPhysicalSetting>))]
    public class PhysicalSettingRepository : SqlRepositoryBase<PhysicalSetting, IPhysicalSetting> { }
}
