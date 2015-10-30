using System;
using System.Collections.Generic;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IPhase : IIdentifiable, ISqlPersistable
    {
        /// <summary>
        /// Get identifier of current match.
        /// </summary>
        Guid ContestId { get; }

        /// <summary>
        /// Get type of current game step.
        /// </summary>
        PhaseType Type { get; }

        /// <summary>
        /// Get team involved in current game step.
        /// </summary>
        IList<ITeam> TeamList { get; set; }

        /// <summary>
        /// Get all game step of current phase.
        /// </summary>
        IList<IGameStep> GameStepList { get; set; }

        /// <summary>
        /// Get actual game step of current phase.
        /// </summary>
        IGameStep ActualGameStep { get; }

        /// <summary>
        /// Get boolean to know if current phase is started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Build all game step and launch current phase
        /// </summary>
        void LaunchNextStep();

        /// <summary>
        /// This event occurs when current phase is ended
        /// </summary>
        event PhaseEvent PhaseEnded;

        /// <summary>
        /// This event occurs when current phase is ended
        /// </summary>
        event NextStepStartedEvent NextStepStarted;
    }
}