using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IEliminationStepSetting>))]
    [Export(typeof(ISqlRepository<IEliminationStepSetting>))]
    public class EliminationStepSettingRepository : SqlRepository<EliminationStepSetting, IEliminationStepSetting> { }
}
