using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Contest.Core.Component;
using Contest.Core.FrameworkExtension;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using Contest.Domain.Settings;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    [Entity(NameInStore = "CONTEST")]
    public class Contest : Entity, IContest
    {
        #region Constructors

        internal Contest()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _gameSetting = new Lazy<IGameSetting>(() =>
                GameSettingRepository.FirstOrDefault(_ => _.Id == GameSettingId));
            _physicalSetting = new Lazy<IPhysicalSetting>(() =>
                PhysicalSettingRepository.FirstOrDefault(_ => _.Id == PhysicalSettingId));
            _fieldList =
                new Lazy<IList<IField>>(() => FieldRepository.Find(_ => _.CurrentContestId == Id).ToList());
            _teamList = new Lazy<IList<ITeam>>(() => TeamRepository.Find(_ => _.ContestId == Id).ToList());
            _eliminationSetting = new Lazy<IEliminationStepSetting>(() =>
                EliminationStepSettingRepository.FirstOrDefault(_ =>
                    _.Id == EliminationSettingId));
            _consolingEliminationSetting = new Lazy<IEliminationStepSetting>(() =>
                EliminationStepSettingRepository.FirstOrDefault(_ =>
                    _.Id == ConsolingEliminationSettingId));
            _qualificationSetting = new Lazy<IQualificationStepSetting>(() =>
                QualificationStepSettingRepository.FirstOrDefault(_ =>
                    _.Id == QualificationSettingId));
            _phaseList = new Lazy<IList<IPhase>>(() => PhaseRepository.Find(_ => _.ContestId == Id).Cast<IPhase>().ToList());
        }

        #endregion

        #region Factory

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Contest" /> with specified param
        /// </summary>
        /// <param name="datePlanned">Date planned for contest.</param>
        /// <param name="physicalSetting">Specify type of physical setting.</param>
        /// <param name="gameSetting">Specify type of game setting.</param>
        /// <returns>Tournament's instance</returns>
        public static IContest Create(DateTime datePlanned, IPhysicalSetting physicalSetting, IGameSetting gameSetting)
        {
            var result = new Contest
            {
                DatePlanned = datePlanned,
                PhysicalSetting = physicalSetting,
                GameSetting = gameSetting,
                EliminationSetting = new EliminationStepSetting(new MatchSetting(), EliminationType.QuarterFinal),
                TeamList = new List<ITeam>(),
                PhaseList = new List<IPhase>()
            };

            return result;
        }

        #endregion

        #region Fields

        private Lazy<IGameSetting> _gameSetting;
        private Lazy<IPhysicalSetting> _physicalSetting;
        private Lazy<IList<IField>> _fieldList;
        private Lazy<IList<ITeam>> _teamList;
        private Lazy<IEliminationStepSetting> _eliminationSetting;
        private Lazy<IEliminationStepSetting> _consolingEliminationSetting;
        private Lazy<IQualificationStepSetting> _qualificationSetting;
        private Lazy<IList<IPhase>> _phaseList;

        #endregion

        #region MEF Import

        [Import] private IRepository<Team, ITeam> TeamRepository { get; set; }

        [Import] private IFieldFactory FieldFactory { get; set; }

        [Import] private IRepository<Field, IField> FieldRepository { get; set; }

        [Import] private IRepository<PhysicalSetting, IPhysicalSetting> PhysicalSettingRepository { get; set; }

        [Import] private IRepository<GameSetting, IGameSetting> GameSettingRepository { get; set; }

        [Import] private IRepository<EliminationStepSetting, IEliminationStepSetting> EliminationStepSettingRepository { get; set; }

        [Import] private IRepository<QualificationStepSetting, IQualificationStepSetting> QualificationStepSettingRepository { get; set; }

        [Import] private IRepository<Phase, IPhase> PhaseRepository { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Get planned date for contest.
        /// </summary>
        [Field(FieldName = "DATE_PLANNED")]
        public DateTime DatePlanned { get; set; }

        /// <summary>
        ///     Get beginning date for contest.
        /// </summary>
        [Field(FieldName = "DATE_BEGUIN")]
        public DateTime? BeginningDate { get; set; }

        /// <summary>
        ///     Indicated if current contest is started
        /// </summary>
        public bool IsStarted => BeginningDate != null;

        /// <summary>
        ///     Indicate if current contest is ended
        /// </summary>
        public bool IsFinished => EndedDate != null;

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "DATE_ENDED")]
        public DateTime? EndedDate { get; set; }

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "GAME_SETTING")]
        public Guid GameSettingId { get; private set; }

        /// <summary>
        ///     Get game setting for current contest.
        /// </summary>
        public IGameSetting GameSetting
        {
            get => _gameSetting.Value;
            set
            {
                _gameSetting = new Lazy<IGameSetting>(() => value);
                GameSettingId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "PHYSICAL_SETTING")]
        public Guid PhysicalSettingId { get; private set; }

        /// <summary>
        ///     Get physical setting for current contest.
        /// </summary>
        public IPhysicalSetting PhysicalSetting
        {
            get => _physicalSetting.Value;
            set
            {
                _physicalSetting = new Lazy<IPhysicalSetting>(() => value);
                PhysicalSettingId = value?.Id ?? Guid.Empty;
            }
        }

        public ushort CountField
        {
            get => PhysicalSetting.CountField;
            set
            {
                if (IsStarted)
                    throw new NotSupportedException(
                        "Number of field can not be changed when contest is already started");
                PhysicalSetting.CountField = value;
            }
        }

        /// <summary>
        ///     Get minimum team that can be register for current contest.
        /// </summary>
        public uint MinimumPlayerByTeam
        {
            get => GameSetting.MinimumPlayerByTeam;
            set
            {
                if (IsStarted)
                    throw new NotSupportedException(
                        "Number of minimum player by team can not be changed when contest is already started");
                GameSetting.MinimumPlayerByTeam = value;
            }
        }

        /// <summary>
        ///     Get maximum team that can be register for current contest.
        /// </summary>
        public uint MaximumPlayerByTeam
        {
            get => GameSetting.MaximumPlayerByTeam;
            set
            {
                if (IsStarted)
                    throw new NotSupportedException(
                        "Number of maximum player by team can not be changed when contest is already started");
                GameSetting.MaximumPlayerByTeam = value;
            }
        }

        /// <summary>
        ///     Get Field list for current contest.
        /// </summary>
        public IList<IField> FieldList => _fieldList.Value;

        /// <summary>
        ///     Get team list for current contest.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get => _teamList.Value;
            private set { _teamList = new Lazy<IList<ITeam>>(() => value); }
        }

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "ELIMINATION_SETTING_ID")]
        public Guid EliminationSettingId { get; private set; }

        /// <summary>
        ///     Get setting for elimination step
        /// </summary>
        public IEliminationStepSetting EliminationSetting
        {
            get => _eliminationSetting.Value;
            set
            {
                if (IsStarted)
                    throw new InvalidOperationException("Can not add setup constest when it is already started.");
                _eliminationSetting = new Lazy<IEliminationStepSetting>(() => value);
                EliminationSettingId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get Principal phase.
        /// </summary>
        public IPhase PrincipalPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Main); }
        }

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "CONSOLING_ELIMINATION_SETTING_ID")]
        public Guid ConsolingEliminationSettingId { get; private set; }

        /// <summary>
        ///     Get setting for consoling elimination step
        /// </summary>
        public IEliminationStepSetting ConsolingEliminationSetting
        {
            get => _consolingEliminationSetting.Value;
            set
            {
                if (IsStarted)
                    throw new InvalidOperationException("Can not add setup constest when it is already started.");
                if (!WithQualificationPhase)
                    throw new InvalidOperationException(
                        "Can not add setup consoling if contest haven't qualification.");
                _consolingEliminationSetting = new Lazy<IEliminationStepSetting>(() => value);
                ConsolingEliminationSettingId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get Consoling phase.
        /// </summary>
        public IPhase ConsolingPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Consoling); }
        }

        /// <summary>
        ///     Get end date for contest.
        /// </summary>
        [Field(FieldName = "QUALIFICATION_SETTING_ID")]
        public Guid QualificationSettingId { get; private set; }

        /// <summary>
        ///     Get setting for qualification step
        /// </summary>
        public IQualificationStepSetting QualificationSetting
        {
            get => _qualificationSetting.Value;
            set
            {
                if (IsStarted)
                    throw new InvalidOperationException("Can not add setup constest when it is already started.");
                if (value == null && ConsolingEliminationSetting != null) ConsolingEliminationSetting = null;
                _qualificationSetting = new Lazy<IQualificationStepSetting>(() => value);
                QualificationSettingId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get qualification phase.
        /// </summary>
        public IPhase QualificationPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Qualification); }
        }

        /// <summary>
        ///     Get all phase of current contest
        /// </summary>
        public IList<IPhase> PhaseList
        {
            get => _phaseList.Value;
            private set { _phaseList = new Lazy<IList<IPhase>>(() => value); }
        }

        /// <summary>
        ///     Set to true to make contest with qualification group.
        /// </summary>
        public bool WithQualificationPhase => QualificationSetting != null;

        /// <summary>
        ///     Set to true for contest with consolante step, false else.
        /// </summary>
        public bool WithConsolante => ConsolingEliminationSetting != null;

        #endregion

        #region Methods

        /// <summary>
        ///     Get boolean to know if specfied team is register to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        public bool IsRegister(ITeam team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));
            return TeamList.Any(item => item == team);
        }

        /// <summary>
        ///     Register specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to register.</param>
        public void Register(ITeam team)
        {
            if (IsRegister(team))
                throw new ArgumentException($"L'équipe spécifié est déjà inscrite au tournoi. Equipe:{team.Name}.");

            if (TeamList.Any(item => string.Equals(item.Name, team.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Une équipe possède déjà le nom spécifié. Nom:{team.Name}.");
            TeamList.Add(team);
        }

        /// <summary>
        ///     Unregister specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        public void UnRegister(ITeam team)
        {
            if (!IsRegister(team))
                throw new ArgumentException(
                    $"L'équipe spécifié n'est pas inscrite au tournoi. Equipe:{team.Name}.");
            TeamList.Remove(team);
        }

        /// <summary>
        ///     Start current contest with qualification step and specified param.
        /// </summary>
        public void StartContest()
        {
            if (EliminationSetting == null)
                throw new NotSupportedException("Elimination setting have to be set before start contest.");

            _fieldList = new Lazy<IList<IField>>(() => CreateField());

            TeamList.Shuffle();

            var newPhase = WithQualificationPhase
                ? Phase.CreateQualificationPhase(this, TeamList, QualificationSetting)
                : Phase.CreateMainEliminationPhase(this, TeamList, EliminationSetting);

            PhaseList.Add(newPhase);
            BeginningDate = DateTime.Now;
            RaiseContestEvent(ContestStart);
        }

        private IList<IField> CreateField()
        {
            var result = new List<IField>();
            for (var i = 1; i <= PhysicalSetting.CountField; i++)
                result.Add(FieldFactory.Create(this, i.ToString(CultureInfo.InvariantCulture)));
            return result;
        }

        public void LaunchNextPhase()
        {
            if (QualificationPhase == null) throw new NotSupportedException();

            foreach (var step in QualificationPhase.GameStepList) step.EndGameStep();

            var directQualifiedTeam = QualificationPhase.GetDirectQualifiedTeam();
            var matchList = QualificationPhase.GetAllMatch();
            IList<ITeam> allQualifiedTeam = directQualifiedTeam.Union(QualificationStep
                .SortRank(TeamList.Where(team => !directQualifiedTeam.Contains(team)).ToList(), matchList)
                .Take(QualificationSetting.CountTeamFished)).ToList();
            allQualifiedTeam.Shuffle();
            var main = Phase.CreateMainEliminationPhase(this, allQualifiedTeam, EliminationSetting);

            PhaseList.Add(main);
            RaiseNewPhaseStartedEvent(NewPhaseLaunch, main);

            if (!WithConsolante) return;

            var numberTeamForFirstConsolingElimination = (int) ConsolingEliminationSetting.FirstStep * 2;

            IList<ITeam> consolingTeam = QualificationStep
                .SortRank(TeamList.Where(_ => !main.TeamList.Contains(_)).ToList(), matchList)
                .Take(numberTeamForFirstConsolingElimination)
                .ToList();
            consolingTeam.Shuffle();

            var consoling = Phase.CreateConsolingEliminationPhase(this, consolingTeam, ConsolingEliminationSetting);

            PhaseList.Add(consoling);
            RaiseNewPhaseStartedEvent(NewPhaseLaunch, consoling);
        }

        public void EndContest()
        {
            if (!IsStarted) throw new NotSupportedException("Le tournoi n'est pas encore démarré.");
            if (PhaseList.Count(_ => !_.IsFinished) != 0)
                throw new NotSupportedException("Toutes les phases du tournoi ne sont pas encore terminé.");

            EndedDate = DateTime.Now;
        }

        #endregion

        #region Static methods

        public static int? MinTeamRegister(int countTeamFirstEliminationStep, bool hasQualificationStep,
            int? countQualificationGroup, bool hasConsolingStep, int? countTeamFirstConsolingEliminationStep)
        {
            if (hasQualificationStep && countQualificationGroup == null) return null;
            if (hasConsolingStep && countTeamFirstConsolingEliminationStep == null) return null;
            if (hasQualificationStep && hasConsolingStep)
                return countTeamFirstEliminationStep + (countQualificationGroup > countTeamFirstConsolingEliminationStep
                           ? countQualificationGroup
                           : countTeamFirstConsolingEliminationStep);
            if (!hasQualificationStep && !hasConsolingStep) return countTeamFirstEliminationStep;
            return countTeamFirstEliminationStep +
                   (hasQualificationStep ? countQualificationGroup : countTeamFirstConsolingEliminationStep);
        }

        public static int? MaxTeamRegister(int countTeamFirstEliminationStep, bool hasQualificationStep,
            int? countQualificationGroup, bool hasConsolingStep, int? countTeamFirstConsolingEliminationStep)
        {
            if (hasQualificationStep && countQualificationGroup == null) return null;
            if (hasConsolingStep && countTeamFirstConsolingEliminationStep == null) return null;
            return !hasQualificationStep ? countTeamFirstEliminationStep : int.MaxValue;
        }

        public static IContest Load(IRepository<Contest, IContest> repo, Guid id)
        {
            return repo.GetAll().FirstOrDefault(_ => _.Id == id);
        }

        #endregion

        #region Event

        private void RaiseContestEvent(ContestEvent @event)
        {
            @event?.Invoke(this);
        }

        private void RaiseNewPhaseStartedEvent(NextPhaseStartedEvent @event, IPhase newPhase)
        {
            @event?.Invoke(this, newPhase);
        }


        public event ContestEvent ContestStart;
        public event NextPhaseStartedEvent NewPhaseLaunch;

        #endregion
    }
}