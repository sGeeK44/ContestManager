using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a Match beween two team
    /// </summary>
    [DataContract(Name = "MATCH")]
    public class Match : Identifiable<Match>, IMatch
    {
        #region Fields

        private Lazy<IGameStep> _gameStep;
        private Lazy<ITeam> _team1;
        private Lazy<ITeam> _team2;
        private Lazy<IField> _matchField;
        private Lazy<IMatchSetting> _setting;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IMatchSetting> MatchSettingRepository { get; set; }

        [Import]
        private IRepository<IField> FieldRepository { get; set; }

        [Import]
        private IRepository<ITeam> TeamRepository { get; set; }

        [Import]
        private IRepository<IGameStep> GameStepRepository { get; set; }

        #endregion

        #region Constructors

        internal Match()
        {
            Id = Guid.NewGuid();
            InitializeLazyProperties();
        }

        internal Match(InternalMatchState state)
            : this()
        {
            CurrentState = state;
        }

        private void InitializeLazyProperties()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _gameStep = new Lazy<IGameStep>(() => GameStepRepository.FirstOrDefault(_ => _.Id == GameStepId));
            _team1 = new Lazy<ITeam>(() => TeamRepository.FirstOrDefault(_ => _.Id == Team1Id));
            _team2 = new Lazy<ITeam>(() => TeamRepository.FirstOrDefault(_ => _.Id == Team2Id));
            _matchField = new Lazy<IField>(() => FieldRepository.FirstOrDefault(_ => _.Id == MatchFieldId));
            _setting = new Lazy<IMatchSetting>(() => MatchSettingRepository.FirstOrDefault(_ => _.Id == SettingId));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get game step id linked
        /// </summary>
        [SqlField(Name = "GAME_STEP_ID")]
        public Guid GameStepId { get; private set; }

        /// <summary>
        /// Get game step linked
        /// </summary>
        public IGameStep GameStep
        {
            get { return _gameStep.Value; }
            internal set
            {
                _gameStep = new Lazy<IGameStep>(() => value);
                GameStepId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get first involved team Id in current match
        /// </summary>
        [SqlField(Name = "TEAM_1_ID")]
        public Guid Team1Id { get; private set; }

        /// <summary>
        /// Get or Set first involved team in current match
        /// </summary>
        public ITeam Team1
        {
            get { return _team1.Value; }
            set
            {
                _team1 = new Lazy<ITeam>(() => value);
                Team1Id = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get first team match score
        /// </summary>
        [SqlField(Name = "TEAM_SCORE_1")]
        public ushort TeamScore1 { get; internal set; }

        /// <summary>
        /// Get second involved team Id in current match
        /// </summary>
        [SqlField(Name = "TEAM_2_ID")]
        public Guid Team2Id { get; private set; }

        /// <summary>
        /// Get or Set second involved team in current match
        /// </summary>
        public ITeam Team2
        {
            get { return _team2.Value; }
            set
            {
                _team2 = new Lazy<ITeam>(() => value);
                Team2Id = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get second team match score
        /// </summary>
        [SqlField(Name = "TEAM_SCORE_2")]
        public ushort TeamScore2 { get; internal set; }

        /// <summary>
        /// Get place Id where match is played
        /// </summary>
        [SqlField(Name = "FIELD_ID")]
        public Guid MatchFieldId { get; private set; }

        /// <summary>
        /// Get place where match is played
        /// </summary>
        public IField MatchField
        {
            get { return _matchField.Value; }
            internal set
            {
                _matchField = new Lazy<IField>(() => value);
                MatchFieldId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get Time of beginning match
        /// </summary>
        [SqlField(Name = "DATE_BEGUIN")]
        public DateTime? Beginning { get; internal set; }

        /// <summary>
        /// Get Time between start and now
        /// </summary>
        public TimeSpan? Elapse
        {
            get
            {
                if (!IsBeginning)
                    return null;

                if (IsFinished)
                    return Endded.GetValueOrDefault() - Beginning.GetValueOrDefault();

                return DateTime.Now - Beginning.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Get Time of end match
        /// </summary>
        [SqlField(Name = "DATE_ENDED")]
        public DateTime? Endded { get; internal set; }

        /// <summary>
        /// Get winner of current match
        /// </summary>
        public ITeam Winner { get { return CurrentState.Winner; } }

        /// <summary>
        /// Get boolean to know if current match is beginning
        /// </summary>
        public bool IsBeginning { get { return CurrentState.IsBeginning; } }

        /// <summary>
        /// Get boolean to know if current match is ended
        /// </summary>
        public bool IsFinished { get { return CurrentState.IsFinished; } }

        /// <summary>
        /// Indicate if current match is closed
        /// </summary>
        public bool IsClose { get { return CurrentState.IsClosed; } }

        /// <summary>
        /// Get boolean to know if current match is ended
        /// </summary>
        [SqlField(Name = "STATE")]
        public MatchState MatchState
        {
            get { return CurrentState.State; }
            internal set
            {
                switch (value)
                {
                    case MatchState.Planned:
                        CurrentState = new PlannedMatchState(this);
                        break;
                    case MatchState.InProgress:
                        CurrentState = new InProgressMatchState(this);
                        break;
                    case MatchState.Finished:
                        CurrentState = new FinishedMatchState(this);
                        break;
                    case MatchState.Closed:
                        CurrentState = new ClosedMatchState(this);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown Match State. State:{0}", value));
                }
            }
        }

        internal InternalMatchState CurrentState { get; set; }

        /// <summary>
        /// Get current match setting Id
        /// </summary>
        [SqlField(Name = "SETTING_ID")]
        public Guid SettingId { get; private set; }

        /// <summary>
        /// Get current match setting
        /// </summary>
        public virtual IMatchSetting Setting
        {
            get { return _setting.Value; }
            internal set
            {
                _setting = new Lazy<IMatchSetting>(() => value);
                SettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public void Start(IField field)
        {
            CurrentState.Start(field);
            RaiseMatchEvent(MatchStarted);
        }

        /// <summary>
        /// Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public void SetResult(ushort teamScore1, ushort teamScore2)
        {
            if (CurrentState.SetResult(teamScore1, teamScore2)) RaiseMatchEvent(ScoreChanged);
            RaiseMatchEvent(MatchEnded);
        }

        /// <summary>
        /// Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public void UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            if (CurrentState.UpdateScore(teamScore1, teamScore2)) RaiseMatchEvent(ScoreChanged);
        }

        public void Close()
        {
            CurrentState.Close();
            RaiseMatchEvent(MatchClosed);
        }

        internal void RaiseMatchEvent(MatchEvent @event)
        {
            if (@event != null) @event(this);
        }

        public bool IsTeamInvolved(ITeam team)
        {
            return Team1 == team || Team2 == team;
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            if (unitOfWorks == null) throw new ArgumentNullException("unitOfWorks");

            unitOfWorks.InsertOrUpdate<IMatch>(this);
        }

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            if (unitOfWorks == null) throw new ArgumentNullException("unitOfWorks");

            unitOfWorks.Delete<IMatch>(this);
        }

        #endregion

        #region Event

        /// <summary>
        /// This event occurs when current match beguin
        /// </summary>
        public event MatchEvent MatchStarted;

        /// <summary>
        /// This event occurs when current match is ended
        /// </summary>
        public event MatchEvent MatchEnded;

        /// <summary>
        /// This event occurs when score of current match changed
        /// </summary>
        public event MatchEvent ScoreChanged;

        /// <summary>
        /// This event occurs when score of current match is closed
        /// </summary>
        public event MatchEvent MatchClosed;

        #endregion
    }
}
