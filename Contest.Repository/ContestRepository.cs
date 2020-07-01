using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<Domain.Games.Contest, IContest>))]
    public class ContestRepository : SqlRepositoryBase<Domain.Games.Contest, IContest> { }
}
