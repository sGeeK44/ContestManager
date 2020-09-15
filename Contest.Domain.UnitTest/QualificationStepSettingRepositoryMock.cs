using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IQualificationStepSetting>))]
    [Export(typeof(IRepository<QualificationStepSetting, IQualificationStepSetting>))]
    public class QualificationStepSettingRepositoryMock : RepositoryBaseMock<QualificationStepSetting, IQualificationStepSetting>
    {
    }
}