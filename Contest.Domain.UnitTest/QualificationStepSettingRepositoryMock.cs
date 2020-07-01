using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<QualificationStepSetting, IQualificationStepSetting>))]
    public class QualificationStepSettingRepositoryMock : RepositoryBaseMock<QualificationStepSetting, IQualificationStepSetting>
    {
    }
}