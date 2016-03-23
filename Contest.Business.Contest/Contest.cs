using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.FrameworkExtension;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [DataContract(Name = "CONTEST")]
    public class Contest : Identifiable<Contest>, IContest
    {
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

        [Import]
        private IRepository<ITeam> TeamRepository { get; set; }

        [Import]
        private IFieldFactory FieldFactory { get; set; }

        [Import]
        private IRepository<IField> FieldRepository { get; set; }

        [Import]
        private IRepository<IPhysicalSetting> PhysicalSettingRepository { get; set; }

        [Import]
        private IRepository<IGameSetting> GameSettingRepository { get; set; }

        [Import]
        private IRepository<IEliminationStepSetting> EliminationStepSettingRepository { get; set; }

        [Import]
        private IRepository<IQualificationStepSetting> QualificationStepSettingRepository { get; set; }

        [Import]
        private IRepository<IPhase> PhaseRepository { get; set; }

        #endregion

        #region Constructors

        internal Contest()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _gameSetting = new Lazy<IGameSetting>(() => GameSettingRepository.FirstOrDefault(_ => _.Id == GameSettingId));
            _physicalSetting = new Lazy<IPhysicalSetting>(() => PhysicalSettingRepository.FirstOrDefault(_ => _.Id == PhysicalSettingId));
            _fieldList = new Lazy<IList<IField>>(() => FieldRepository.Find(_ => _.CurrentContestId == Id).ToList());
            _teamList = new Lazy<IList<ITeam>>(() => TeamRepository.Find(_ => _.ContestId == Id).ToList());
            _eliminationSetting = new Lazy<IEliminationStepSetting>(() => EliminationStepSettingRepository.FirstOrDefault(_ => _.Id == EliminationSettingId));
            _consolingEliminationSetting = new Lazy<IEliminationStepSetting>(() => EliminationStepSettingRepository.FirstOrDefault(_ => _.Id == ConsolingEliminationSettingId));
            _qualificationSetting = new Lazy<IQualificationStepSetting>(() => QualificationStepSettingRepository.FirstOrDefault(_ => _.Id == QualificationSettingId));
            _phaseList = new Lazy<IList<IPhase>>(() => PhaseRepository.Find(_ => _.ContestId == Id).ToList());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get planned date for contest.
        /// </summary>
        [DataMember(Name = "DATE_PLANNED")]
        public DateTime DatePlanned { get; set; }

        /// <summary>
        /// Get beginning date for contest.
        /// </summary>
        [DataMember(Name = "DATE_BEGUIN")]
        public DateTime? BeginningDate { get; set; }

        /// <summary>
        /// Indicated if current contest is started
        /// </summary>
        public bool IsStarted
        {
            get { return BeginningDate != null; }
        }

        /// <summary>
        /// Indicate if current contest is ended
        /// </summary>
        public bool IsFinished
        {
            get { return EndedDate != null; }
        }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "DATE_ENDED")]
        public DateTime? EndedDate { get; set; }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "GAME_SETTING")]
        public Guid GameSettingId { get; private set; }

        /// <summary>
        /// Get game setting for current contest.
        /// </summary>
        public IGameSetting GameSetting
        {
            get { return _gameSetting.Value; }
            set
            {
                _gameSetting = new Lazy<IGameSetting>(() => value);
                GameSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "PHYSICAL_SETTING")]
        public Guid PhysicalSettingId { get; private set; }

        /// <summary>
        /// Get physical setting for current contest.
        /// </summary>
        public IPhysicalSetting PhysicalSetting
        {
            get { return _physicalSetting.Value; }
            set
            {
                _physicalSetting = new Lazy<IPhysicalSetting>(() => value);
                PhysicalSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        public ushort CountField
        {
            get { return PhysicalSetting.CountField; }
            set
            {
                if (IsStarted) throw new NotSupportedException("Number of field can not be changed when contest is already started");
                PhysicalSetting.CountField = value;
            }
        }

        /// <summary>
        /// Get minimum team that can be register for current contest.
        /// </summary>
        public uint MinimumPlayerByTeam
        {
            get { return GameSetting.MinimumPlayerByTeam; }
            set
            {
                if (IsStarted) throw new NotSupportedException("Number of minimum player by team can not be changed when contest is already started");
                GameSetting.MinimumPlayerByTeam = value;
            }
        }

        /// <summary>
        /// Get maximum team that can be register for current contest.
        /// </summary>
        public uint MaximumPlayerByTeam
        {
            get { return GameSetting.MaximumPlayerByTeam; }
            set
            {
                if (IsStarted) throw new NotSupportedException("Number of maximum player by team can not be changed when contest is already started");
                GameSetting.MaximumPlayerByTeam = value;
            }
        }

        /// <summary>
        /// Get Field list for current contest.
        /// </summary>
        public IList<IField> FieldList
        {
            get { return _fieldList.Value; }
        }

        /// <summary>
        /// Get team list for current contest.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get { return _teamList.Value; }
            private set { _teamList = new Lazy<IList<ITeam>>(() => value); }
        }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "ELIMINATION_SETTING_ID")]
        public Guid EliminationSettingId { get; private set; }

        /// <summary>
        /// Get setting for elimination step
        /// </summary>
        public IEliminationStepSetting EliminationSetting
        {
            get { return _eliminationSetting.Value; }
            set
            {
                if (IsStarted) throw new InvalidOperationException("Can not add setup constest when it is already started.");
                _eliminationSetting = new Lazy<IEliminationStepSetting>(() => value);
                EliminationSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get Principal phase.
        /// </summary>
        public IPhase PrincipalPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Main); }
        }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "CONSOLING_ELIMINATION_SETTING_ID")]
        public Guid ConsolingEliminationSettingId { get; private set; }

        /// <summary>
        /// Get setting for consoling elimination step
        /// </summary>
        public IEliminationStepSetting ConsolingEliminationSetting
        {
            get { return _consolingEliminationSetting.Value; }
            set
            {
                if (IsStarted) throw new InvalidOperationException("Can not add setup constest when it is already started.");
                if (!WithQualificationPhase) throw new InvalidOperationException("Can not add setup consoling if contest haven't qualification.");
                _consolingEliminationSetting = new Lazy<IEliminationStepSetting>(()=>value);
                ConsolingEliminationSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get Consoling phase.
        /// </summary>
        public IPhase ConsolingPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Consoling); }
        }

        /// <summary>
        /// Get end date for contest.
        /// </summary>
        [DataMember(Name = "QUALIFICATION_SETTING_ID")]
        public Guid QualificationSettingId { get; private set; }

        /// <summary>
        /// Get setting for qualification step
        /// </summary>
        public IQualificationStepSetting QualificationSetting
        {
            get { return _qualificationSetting.Value; }
            set
            {
                if (IsStarted) throw new InvalidOperationException("Can not add setup constest when it is already started.");
                if (value == null && ConsolingEliminationSetting != null) ConsolingEliminationSetting = null;
                _qualificationSetting = new Lazy<IQualificationStepSetting>(()=>value);
                QualificationSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get qualification phase.
        /// </summary>
        public IPhase QualificationPhase
        {
            get { return PhaseList.FirstOrDefault(item => item.Type == PhaseType.Qualification); }
        }

        /// <summary>
        /// Get all phase of current contest
        /// </summary>
        public IList<IPhase> PhaseList
        {
            get { return _phaseList.Value; }
            private set { _phaseList = new Lazy<IList<IPhase>>(()=>value); }
        }

        /// <summary>
        /// Set to true to make contest with qualification group.
        /// </summary>
        public bool WithQualificationPhase
        {
            get { return QualificationSetting != null; }
        }

        /// <summary>
        /// Set to true for contest with consolante step, false else.
        /// </summary>
        public bool WithConsolante
        {
            get { return ConsolingEliminationSetting != null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get boolean to know if specfied team is register to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        public bool IsRegister(ITeam team)
        {
            if (team == null) throw new ArgumentNullException("team");
            return TeamList.Any(item => item == team);
        }

        /// <summary>
        /// Register specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to register.</param>
        public void Register(ITeam team)
        {
            if (IsRegister(team)) throw new ArgumentException(string.Format("L'équipe spécifié est déjà inscrite au tournoi. Equipe:{0}.", team.Name));

            if (TeamList.Any(item => string.Equals(item.Name, team.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException(string.Format("Une équipe possède déjà le nom spécifié. Nom:{0}.", team.Name));
            TeamList.Add(team);
        }

        /// <summary>
        /// Unregister specfied team to current contest.
        /// </summary>
        /// <param name="team">Team to unregister.</param>
        public void UnRegister(ITeam team)
        {
            if (!IsRegister(team)) throw new ArgumentException(string.Format("L'équipe spécifié n'est pas inscrite au tournoi. Equipe:{0}.", team.Name));
            TeamList.Remove(team);
        }

        /// <summary>
        /// Start current contest with qualification step and specified param.
        /// </summary>
        public void StartContest()
        {
            if (EliminationSetting == null) throw new NotSupportedException("Elimination setting have to be set before start contest.");

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

            foreach (var step in QualificationPhase.GameStepList)
            {
                step.EndGameStep();
            }

            IList<ITeam> directQualifiedTeam = QualificationPhase.GetDirectQualifiedTeam();
            IList<IMatch> matchList = QualificationPhase.GetAllMatch();
            IList<ITeam> allQualifiedTeam = directQualifiedTeam.Union(QualificationStep.SortRank(TeamList.Where(team => !directQualifiedTeam.Contains(team)).ToList(), matchList)
                                                                                       .Take(QualificationSetting.CountTeamFished)).ToList();
            allQualifiedTeam.Shuffle();
            var main = Phase.CreateMainEliminationPhase(this, allQualifiedTeam, EliminationSetting);

            PhaseList.Add(main);
            RaiseNewPhaseStartedEvent(NewPhaseLaunch, main);

            if (!WithConsolante) return;
            
            var numberTeamForFirstConsolingElimination = (int)ConsolingEliminationSetting.FirstStep*2;

            IList<ITeam> consolingTeam = QualificationStep.SortRank(TeamList.Where(_ => !main.TeamList.Contains(_)).ToList(), matchList)
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
            if (PhaseList.Count(_ => !_.IsFinished) != 0) throw new NotSupportedException("Toutes les phases du tournoi ne sont pas encore terminé.");

            EndedDate = DateTime.Now;
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            if (unitOfWorks == null) throw new ArgumentNullException();

            unitOfWorks.InsertOrUpdate<IContest>(this);
            if (GameSetting != null) GameSetting.PrepareCommit(unitOfWorks);
            if (PhysicalSetting != null) PhysicalSetting.PrepareCommit(unitOfWorks);
            if (EliminationSetting != null) EliminationSetting.PrepareCommit(unitOfWorks);
            if (ConsolingEliminationSetting != null) ConsolingEliminationSetting.PrepareCommit(unitOfWorks);
            if (QualificationSetting != null) QualificationSetting.PrepareCommit(unitOfWorks);

            foreach (var team in TeamList)
            {
                team.PrepareCommit(unitOfWorks);
            }

            foreach (var phase in PhaseList)
            {
                phase.PrepareCommit(unitOfWorks);
            }
            foreach (var field in FieldList)
            {
                field.PrepareCommit(unitOfWorks);
            }
        }

        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            if (unitOfWorks == null) throw new ArgumentNullException();

            unitOfWorks.Delete<IContest>(this);
            if (GameSetting != null) GameSetting.PrepareDelete(unitOfWorks);
            if (PhysicalSetting != null) PhysicalSetting.PrepareDelete(unitOfWorks);
            if (EliminationSetting != null) EliminationSetting.PrepareDelete(unitOfWorks);
            if (ConsolingEliminationSetting != null) ConsolingEliminationSetting.PrepareDelete(unitOfWorks);
            if (QualificationSetting != null) QualificationSetting.PrepareDelete(unitOfWorks);

            foreach (var team in TeamList)
            {
                team.PrepareDelete(unitOfWorks);
            }

            foreach (var phase in PhaseList)
            {
                phase.PrepareDelete(unitOfWorks);
            }
            foreach (var field in FieldList)
            {
                field.PrepareDelete(unitOfWorks);
            }
        }

        #endregion

        #region Static methods

        public static int? MinTeamRegister(int countTeamFirstEliminationStep, bool hasQualificationStep, int? countQualificationGroup, bool hasConsolingStep, int? countTeamFirstConsolingEliminationStep)
        {
            if (hasQualificationStep && countQualificationGroup == null) return null;
            if (hasConsolingStep && countTeamFirstConsolingEliminationStep == null) return null;
            if (hasQualificationStep && hasConsolingStep)
            {
                return countTeamFirstEliminationStep + (countQualificationGroup > countTeamFirstConsolingEliminationStep ? countQualificationGroup : countTeamFirstConsolingEliminationStep);
            }
            if (!hasQualificationStep && !hasConsolingStep) return countTeamFirstEliminationStep;
            return countTeamFirstEliminationStep + (hasQualificationStep ? countQualificationGroup : countTeamFirstConsolingEliminationStep);
            
        }

        public static int? MaxTeamRegister(int countTeamFirstEliminationStep, bool hasQualificationStep, int? countQualificationGroup, bool hasConsolingStep, int? countTeamFirstConsolingEliminationStep)
        {
            if (hasQualificationStep && countQualificationGroup == null) return null;
            if (hasConsolingStep && countTeamFirstConsolingEliminationStep == null) return null;
            return !hasQualificationStep ? countTeamFirstEliminationStep : int.MaxValue;
        }

        public static IContest Load(IRepository<IContest> repo, Guid id)
        {
            return repo.FirstOrDefault(_ => _.Id == id);
        }

        #endregion

        #region Event

        private void RaiseContestEvent(ContestEvent @event)
        {
            if (@event != null) @event(this);
        }

        private void RaiseNewPhaseStartedEvent(NextPhaseStartedEvent @event, IPhase newPhase)
        {
            if (@event != null) @event(this, newPhase);
        }


        public event ContestEvent ContestStart;
        public event NextPhaseStartedEvent NewPhaseLaunch;

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Contest"/> with specified param
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
    }
}
