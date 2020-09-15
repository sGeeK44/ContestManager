using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IPhase>))]
    [Export(typeof(IRepository<Phase, IPhase>))]
    public class PhaseRepository : SqlRepositoryBase<Phase, IPhase> { }
}
