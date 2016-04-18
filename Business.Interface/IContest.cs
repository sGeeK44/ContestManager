using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IContest: IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get planned date for contest.
        /// </summary>
        DateTime DatePlanned { get; set; }

        /// <summary>
        /// Get beginning date for contest.
        /// </summary>
        DateTime? BeginningDate { get; set; }

        /// <summary>
        /// Indicated if current contest is started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Indicate if current contest is ended
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        DateTime? EndedDate { get; set; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        Guid GameSettingId { get; }

        /// <summary>
        /// Get game setting for current contest.
        /// </summary>
        IGameSetting GameSetting { get; set; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        Guid PhysicalSettingId { get; }

        /// <summary>
        /// Get physical setting for current contest.
        /// </summary>
        IPhysicalSetting PhysicalSetting { get; set; }

        /// <summary>
        /// Get count available for current contest
        /// </summary>
        ushort CountField { get; set; }

        /// <summary>
        /// Get minimum team that can be register for current contest.
        /// </summary>
        uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        /// Get maximum team that can be register for current contest.
        /// </summary>
        uint MaximumPlayerByTeam { get; set; }

        /// <summary>
        /// Get Field list for current contest.
        /// </summary>
        IList<IField> FieldList { get; }

        /// <summary>
        /// Get team list for current contest.
        /// </summary>
        IList<ITeam> TeamList { get; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        Guid EliminationSettingId { get; }

        /// <summary>
        /// Get setting for elimination step
        /// </summary>
        IEliminationStepSetting EliminationSetting { get; set; }

        /// <summary>
        /// Get Principal phase.
        /// </summary>
        IPhase PrincipalPhase { get; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        Guid ConsolingEliminationSettingId { get; }

        /// <summary>
        /// Get setting for consoling elimination step
        /// </summary>
        IEliminationStepSetting ConsolingEliminationSetting { get; set; }

        /// <summary>
        /// Get Consoling phase.
        /// </summary>
        IPhase ConsolingPhase { get; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        Guid QualificationSettingId { get; }

        /// <summary>
        /// Get setting for qualification step
        /// </summary>
        IQualificationStepSetting QualificationSetting { get; set; }

        /// <summary>
        /// Get qualification phase.
        /// </summary>
        IPhase QualificationPhase { get; }

        /// <summary>
        /// Get all phase of current contest
        /// </summary>
        IList<IPhase> PhaseList { get; }

        /// <summary>
        /// Set to true to make contest with qualification group.
        /// </summary>
        bool WithQualificationPhase { get; }

        /// <summary>
        /// Set to true for contest with consolante step, false else.
        /// </summary>
        bool WithConsolante { get; }

        /// <summary>
        /// Get boolean to know if specfied team is register to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        bool IsRegister(ITeam team);

        /// <summary>
        /// Register specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to register.</param>
        void Register(ITeam team);

        /// <summary>
        /// Unregister specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        void UnRegister(ITeam team);

        /// <summary>
        /// Start current contest with qualification step and specified param.
        /// </summary>
        void StartContest();

        /// <summary>
        /// Launch next phase for current contest.
        /// </summary>
        void LaunchNextPhase();

        /// <summary>
        /// End current contest.
        /// </summary>
        void EndContest();

        /// <summary>
        /// Occurs when current contest is started
        /// </summary>
        event ContestEvent ContestStart;

        /// <summary>
        /// Occurs when a new phase is launch
        /// </summary>
        event NextPhaseStartedEvent NewPhaseLaunch;
    }
}