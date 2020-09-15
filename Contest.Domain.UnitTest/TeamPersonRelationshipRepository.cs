using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPerson>>))]
    [Export(typeof(IRepository<TeamPersonRelationship, IRelationship<ITeam, IPerson>>))]
    public class TeamPersonRelationshipRepository : RepositoryBaseMock<TeamPersonRelationship, IRelationship<ITeam, IPerson>>
    {
    }
}