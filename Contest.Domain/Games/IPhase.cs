using System;
using System.Collections.Generic;
using Contest.Domain.Matchs;
using Contest.Domain.Players;

namespace Contest.Domain.Games
{
    public interface IPhase : IEntity
    {
        /// <summary>
        ///     Get identifier of current match.
        /// </summary>
        Guid ContestId { get; }

        /// <summary>
        ///     Get type of current game step.
        /// </summary>
        PhaseType Type { get; }

        /// <summary>
        ///     Get team involved in current game step.
        /// </summary>
        IList<ITeam> TeamList { get; set; }

        /// <summary>
        ///     Get all game step of current phase.
        /// </summary>
        IList<IGameStep> GameStepList { get; set; }

        /// <summary>
        ///     Get actual game step of current phase.
        /// </summary>
        IGameStep ActualGameStep { get; }

        /// <summary>
        ///     Get boolean to know if current phase is started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        ///     Get boolean to know if current game step is finished.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        ///     Build all game step and launch current phase
        /// </summary>
        void LaunchNextStep();

        /// <summary>
        ///     This event occurs when current phase is ended
        /// </summary>
        event PhaseEvent PhaseEnded;

        /// <summary>
        ///     This event occurs when current phase is ended
        /// </summary>
        event NextStepStartedEvent NextStepStarted;

        /// <summary>
        ///     Return Team whose directly qualified for next phase
        /// </summary>
        /// <returns></returns>
        IList<ITeam> GetDirectQualifiedTeam();

        /// <summary>
        ///     Return all match of current Phase
        /// </summary>
        /// <returns></returns>
        IList<IMatch> GetAllMatch();
    }
}