using System.ComponentModel.Composition;
using Contest.Business;
using Contest.Core.Repository;

namespace Contest.UnitTest.Kit
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPhase>>))]
    public class RepositoryIRelationshipTeamPhaseMock : RepositoryBaseMock<IRelationship<ITeam, IPhase>>
    { }
}