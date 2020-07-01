using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<GameStep, IGameStep>))]
    public class GameStepRepository : SqlRepositoryBase<GameStep, IGameStep> { }
}
