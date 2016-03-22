using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IGameStep>))]
    public class RepositoryGameStepMock : RepositoryBaseMock<IGameStep>
    { }
}