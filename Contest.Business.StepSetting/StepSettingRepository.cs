using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IStepSetting>))]
    [Export(typeof(ISqlRepository<IStepSetting>))]
    public class StepSettingRepository : SqlRepositoryBase<StepSetting, IStepSetting> { }
}
