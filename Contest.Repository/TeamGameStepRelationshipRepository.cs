﻿using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;
using Contest.Domain.Players;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IRelationship<ITeam, IGameStep>>))]
    [Export(typeof(IRepository<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>>))]
    public class TeamGameStepRelationshipRepository : SqlRepositoryBase<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>> { }
}
