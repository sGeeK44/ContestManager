using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IEliminationStep>))]
    [Export(typeof(ISqlRepository<IEliminationStep>))]
    public class EliminationStepRepository : SqlRepository<EliminationStep, IEliminationStep> { }
}
