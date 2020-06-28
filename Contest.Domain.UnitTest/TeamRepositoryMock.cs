using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof (IRepository<ITeam>))]
    public class TeamRepositoryMock : RepositoryBaseMock<ITeam>
    {
    }
}