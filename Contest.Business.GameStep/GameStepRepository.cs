using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Export(typeof(IRepository<IGameStep>))]
    [Export(typeof(ISqlRepository<IGameStep>))]
    public class GameStepRepository : SqlRepositoryBase<GameStep, IGameStep> { }
}
