using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IContest>))]
    [Export(typeof(IRepository<Domain.Games.Contest, IContest>))]
    public class ContestRepository : SqlRepositoryBase<Domain.Games.Contest, IContest> { }
}
