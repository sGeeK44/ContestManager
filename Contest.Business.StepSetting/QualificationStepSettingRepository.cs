using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IQualificationStepSetting>))]
    [Export(typeof(ISqlRepository<IQualificationStepSetting>))]
    public class QualificationStepSettingRepository : SqlRepositoryBase<QualificationStepSetting, IQualificationStepSetting> { }
}
