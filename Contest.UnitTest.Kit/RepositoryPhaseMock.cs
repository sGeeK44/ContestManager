using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IPhase>))]
    public class RepositoryPhaseMock : RepositoryBaseMock<IPhase>
    { }
}