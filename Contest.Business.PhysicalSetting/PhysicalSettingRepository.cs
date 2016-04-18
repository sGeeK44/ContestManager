using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IPhysicalSetting>))]
    [Export(typeof(ISqlRepository<IPhysicalSetting>))]
    public class PhysicalSettingRepository : SqlRepositoryBase<PhysicalSetting, IPhysicalSetting> { }
}
