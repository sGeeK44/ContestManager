using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;

namespace Contest.Business.UnitTest
{
    [Export(typeof (IRepository<Team, ITeam>))]
    public class TeamRepositoryMock : RepositoryBaseMock<Team, ITeam>
    {
    }
}