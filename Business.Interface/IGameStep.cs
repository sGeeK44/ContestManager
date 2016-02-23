using System;
using System.Collections.Generic;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IGameStep : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get phase Id linked.
        /// </summary>
        Guid PhaseId { get; }

        /// <summary>
        /// Get phase linked.
        /// </summary>
        IPhase Phase { get; }

        /// <summary>
        /// Get game step name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return next step, null if no futher step
        /// </summary>
        EliminationType? NextStep { get; }

        /// <summary>
        /// Get setting game Id for current step
        /// </summary>
        Guid CurrentMatchSettingId { get; }

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

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        bool IsMatchListComplete { get; }

        /// <summary>
        /// Build all match for current game step
        /// </summary>
        void BuildMatch();

        /// <summary>
        /// End current game step
        /// </summary>
        void EndGameStep();

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        event GameStepEvent GameStepEnded;

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        event GameStepEvent MatchListComplete;

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        event GameStepEvent RankChanged;
    }
}