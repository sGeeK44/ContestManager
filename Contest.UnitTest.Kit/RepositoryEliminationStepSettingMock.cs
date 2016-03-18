using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IEliminationStepSetting>))]
    public class RepositoryEliminationStepSettingMock : RepositoryBaseMock<IEliminationStepSetting>
    { }
}