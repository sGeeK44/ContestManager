using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IStepSetting>))]
    public class StepSettingRepository : SqlRepositoryBase<StepSetting, IStepSetting> { }
}
