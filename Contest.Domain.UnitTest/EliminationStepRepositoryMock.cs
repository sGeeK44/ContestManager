using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<EliminationStep, IEliminationStep>))]
    public class EliminationStepRepositoryMock : RepositoryBaseMock<EliminationStep, IEliminationStep>
    {
    }
}