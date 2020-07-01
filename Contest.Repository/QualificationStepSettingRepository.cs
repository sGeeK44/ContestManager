using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<QualificationStepSetting, IQualificationStepSetting>))]
    public class QualificationStepSettingRepository : SqlRepositoryBase<QualificationStepSetting, IQualificationStepSetting> { }
}
