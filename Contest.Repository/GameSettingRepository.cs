﻿using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IGameSetting>))]
    public class GameSettingRepository : SqlRepositoryBase<GameSetting, IGameSetting> { }
}