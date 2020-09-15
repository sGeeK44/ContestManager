using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<ITeam>))]
    [Export(typeof (IRepository<Team, ITeam>))]
    public class TeamRepositoryMock : RepositoryBaseMock<Team, ITeam>
    {
    }
}