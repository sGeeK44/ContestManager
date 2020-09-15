using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IEliminationStep>))]
    [Export(typeof(IRepository<EliminationStep, IEliminationStep>))]
    public class EliminationStepRepository : SqlRepositoryBase<EliminationStep, IEliminationStep> { }
}
