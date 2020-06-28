﻿using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Players;

namespace Contest.Repository
{
    [Export(typeof(IRepository<ITeam>))]
    public class TeamRepository : SqlRepositoryBase<Team, ITeam> { }
}