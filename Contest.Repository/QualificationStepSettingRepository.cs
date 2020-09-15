using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IQualificationStepSetting>))]
    [Export(typeof(IRepository<QualificationStepSetting, IQualificationStepSetting>))]
    public class QualificationStepSettingRepository : SqlRepositoryBase<QualificationStepSetting, IQualificationStepSetting> { }
}
