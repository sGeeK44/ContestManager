using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IEliminationStepSetting>))]
    [Export(typeof(IRepository<EliminationStepSetting, IEliminationStepSetting>))]
    public class EliminationStepSettingRepository : SqlRepositoryBase<EliminationStepSetting, IEliminationStepSetting> { }
}
