using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IContest>))]
    [Export(typeof(ISqlRepository<IContest>))]
    public class ContestRepository : SqlRepositoryBase<Contest, IContest> { }
}
