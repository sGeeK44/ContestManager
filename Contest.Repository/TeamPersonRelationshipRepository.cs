using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IRelationship<ITeam, IPerson>>))]
    [Export(typeof(IRepository<TeamPersonRelationship, IRelationship<ITeam, IPerson>>))]
    public class TeamPersonRelationshipRepository : SqlRepositoryBase<TeamPersonRelationship, IRelationship<ITeam, IPerson>> { }
}