using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<EliminationStep, IEliminationStep>))]
    public class EliminationStepRepository : SqlRepositoryBase<EliminationStep, IEliminationStep> { }
}
