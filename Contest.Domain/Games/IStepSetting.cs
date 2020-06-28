using System;
using Contest.Domain.Matchs;

namespace Contest.Domain.Games
{
    public interface IStepSetting : IEntity
    {
        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        Guid MatchSettingId { get; }

        /// <summary>
        ///     Get match's setting for  step
        /// </summary>
        IMatchSetting MatchSetting { get; }
    }
}