using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<StepSetting, IStepSetting>))]
    public class StepSettingRepository : SqlRepositoryBase<StepSetting, IStepSetting> { }
}
