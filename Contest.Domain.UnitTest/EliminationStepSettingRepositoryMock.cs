using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IEliminationStepSetting>))]
    [Export(typeof(IRepository<EliminationStepSetting, IEliminationStepSetting>))]
    public class EliminationStepSettingRepositoryMock : RepositoryBaseMock<EliminationStepSetting, IEliminationStepSetting>
    {
    }
}