using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IPhase>))]
    [Export(typeof(ISqlRepository<IPhase>))]
    public class PhaseRepository : SqlRepository<Phase, IPhase> { }
}
