using System.Collections.Generic;

namespace Contest.Business
{
    public interface IEliminationStep : IGameStep
    {
        /// <summary>
        /// Get type of current game step.
        /// </summary>
        EliminationType Type { get; }

        /// <summary>
        /// Get setting game for current step
        /// </summary>
        IMatchSetting CurrentMatchSetting { get; }

        /// <summary>
        /// Get team involved in current game step.
        /// </summary>
        IList<ITeam> TeamList { get; set; }

        /// <summary>
        /// Get all match of current game step.
        /// </summary>
        IList<IMatch> MatchList { get; }
    }
}