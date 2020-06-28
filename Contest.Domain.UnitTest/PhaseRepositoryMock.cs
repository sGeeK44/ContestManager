using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IPhase>))]
    public class PhaseRepositoryMock : RepositoryBaseMock<IPhase>
    {
    }
}