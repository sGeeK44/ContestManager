using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IEliminationStep>))]
    public class RepositoryEliminationStepMock : RepositoryBaseMock<IEliminationStep>
    { }
}