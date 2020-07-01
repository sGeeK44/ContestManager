using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<EliminationStepSetting, IEliminationStepSetting>))]
    public class EliminationStepSettingRepositoryMock : RepositoryBaseMock<EliminationStepSetting, IEliminationStepSetting>
    {
    }
}