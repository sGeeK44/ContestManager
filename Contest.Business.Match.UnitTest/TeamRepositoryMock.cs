using System.ComponentModel.Composition;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;
using Contest.UnitTest.Kit;

namespace Contest.Business.UnitTest
{
    [Export(typeof (IRepository<ITeam>))]
    [Export(typeof (ISqlRepository<ITeam>))]
    public class TeamRepositoryMock : SqlRepositoryBaseMock<ITeam>, ISqlRepository<ITeam>
    {
    }
}