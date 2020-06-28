using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IPhase>))]
    public class PhaseRepository : SqlRepositoryBase<Phase, IPhase> { }
}
