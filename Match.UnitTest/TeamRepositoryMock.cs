using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business.UnitTest
{
    [Export(typeof (IRepository<ITeam>))]
    [Export(typeof (ISqlRepository<ITeam>))]
    public class TeamRepositoryMock : RepositoryMockBase<ITeam>, ISqlRepository<ITeam>
    {
    }
}