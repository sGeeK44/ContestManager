﻿using System.ComponentModel.Composition;
using Contest.Domain;
using Contest.Domain.Games;

namespace Contest.Repository
{
    [Export(typeof(IRepository<EliminationStepSetting, IEliminationStepSetting>))]
    public class EliminationStepSettingRepository : SqlRepositoryBase<EliminationStepSetting, IEliminationStepSetting> { }
}
