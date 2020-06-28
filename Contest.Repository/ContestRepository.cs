using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IContest>))]
    public class ContestRepository : SqlRepositoryBase<Domain.Games.Contest, IContest> { }
}
