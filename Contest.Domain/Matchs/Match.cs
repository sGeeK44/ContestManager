using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Players;
using Contest.Domain.Settings;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;
using SmartWay.Orm.Repositories;

namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent a Match beween two team
    /// </summary>
    [Entity(NameInStore = "MATCH")]
    public class Match : Entity, IMatch
    {
        #region Fields

        private ReferenceHolder<ITeam, Guid> _team1;
        private ReferenceHolder<ITeam, Guid> _team2;
        private ReferenceHolder<IField, Guid> _matchField;
        private ReferenceHolder<IMatchSetting, Guid> _setting;
        private ReferenceHolder<IGameStep, Guid> _gameStep;

        #endregion

        #region MEF Import

        [Import] private IRepository<MatchSetting, IMatchSetting> MatchSettingRepository { get; set; }

        [Import] private IRepository<Field, IField> FieldRepository { get; set; }

        [Import] private IRepository<Team, ITeam> TeamRepository { get; set; }

        [Import] private IRepository<IGameStep> GameStepRepository { get; set; }

        #endregion

        #region Constructors

        public Match()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _gameStep = new ReferenceHolder<IGameStep, Guid>(GameStepRepository);
            _team1 = new ReferenceHolder<ITeam, Guid>(TeamRepository);
            _team2 = new ReferenceHolder<ITeam, Guid>(TeamRepository);
            _matchField = new ReferenceHolder<IField, Guid>(FieldRepository);
            _setting = new ReferenceHolder<IMatchSetting, Guid>(MatchSettingRepository);
        }

        internal Match(InternalMatchState state)
            : this()
        {
            CurrentState = state;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get game step id linked
        /// </summary>
        [Field(FieldName = "GAME_STEP_ID")]
        public Guid GameStepId
        {
            get => _gameStep.Id;
            set => _gameStep.Id = value;
        }
        public IGameStep GameStep
        {
            get => _gameStep.Object;
            set => _gameStep.Object = value;
        }

        /// <summary>
        ///     Get first involved team Id in current match
        /// </summary>
        [Field(FieldName = "TEAM_1_ID")]
        public Guid Team1Id
        {
            get => _team1.Id;
            private set => _team1.Id = value;
        }

        /// <summary>
        ///     Get or Set first involved team in current match
        /// </summary>
        public ITeam Team1
        {
            get => _team1.Object;
            set => _team1.Object = value;
        }

        /// <summary>
        ///     Get first team match score
        /// </summary>
        [Field(FieldName = "TEAM_SCORE_1")]
        public ushort TeamScore1 { get; internal set; }

        /// <summary>
        ///     Get second involved team Id in current match
        /// </summary>
        [Field(FieldName = "TEAM_2_ID")]
        public Guid Team2Id
        {
            get => _team2.Id;
            private set => _team2.Id = value;
        }

        /// <summary>
        ///     Get or Set second involved team in current match
        /// </summary>
        public ITeam Team2
        {
            get => _team2.Object;
            set => _team2.Object = value;
        }

        /// <summary>
        ///     Get second team match score
        /// </summary>
        [Field(FieldName = "TEAM_SCORE_2")]
        public ushort TeamScore2 { get; internal set; }

        /// <summary>
        ///     Get place Id where match is played
        /// </summary>
        [Field(FieldName = "FIELD_ID")]
        public Guid MatchFieldId
        {
            get => _matchField.Id;
            private set => _matchField.Id = value;
        }

        /// <summary>
        ///     Get place where match is played
        /// </summary>
        public IField MatchField
        {
            get => _matchField.Object;
            set => _matchField.Object = value;
        }

        /// <summary>
        ///     Get Time of beginning match
        /// </summary>
        [Field(FieldName = "DATE_BEGUIN")]
        public DateTime? Beginning { get; internal set; }

        /// <summary>
        ///     Get Time between start and now
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
        ///     Get Time of end match
        /// </summary>
        [Field(FieldName = "DATE_ENDED")]
        public DateTime? Endded { get; internal set; }

        /// <summary>
        ///     Get winner of current match
        /// </summary>
        public ITeam Winner => CurrentState.Winner;

        /// <summary>
        ///     Get boolean to know if current match is beginning
        /// </summary>
        public bool IsBeginning => CurrentState.IsBeginning;

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        public bool IsFinished => CurrentState.IsFinished;

        /// <summary>
        ///     Indicate if current match is closed
        /// </summary>
        public bool IsClose => CurrentState.IsClosed;

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        [Field(FieldName = "STATE")]
        public MatchState MatchState
        {
            get => CurrentState.State;
            set
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
                        throw new InvalidOperationException($"Unknown Match State. State:{value}");
                }
            }
        }

        public InternalMatchState CurrentState { get; set; }

        /// <summary>
        ///     Get current match setting Id
        /// </summary>
        [Field(FieldName = "SETTING_ID")]
        public Guid SettingId
        {
            get => _setting.Id;
            private set => _setting.Id = value;
        }

        /// <summary>
        ///     Get current match setting
        /// </summary>
        public virtual IMatchSetting Setting
        {
            get => _setting.Object;
            set => _setting.Object = value;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public void Start(IField field)
        {
            CurrentState.Start(field);
            RaiseMatchEvent(MatchStarted);
        }

        /// <summary>
        ///     Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public void SetResult(ushort teamScore1, ushort teamScore2)
        {
            if (CurrentState.SetResult(teamScore1, teamScore2)) RaiseMatchEvent(ScoreChanged);
            RaiseMatchEvent(MatchEnded);
        }

        /// <summary>
        ///     Update score of current match by specified value
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
            @event?.Invoke(this);
        }

        public bool IsTeamInvolved(ITeam team)
        {
            return Team1.Equals(team) || Team2.Equals(team);
        }

        #endregion

        #region Event

        /// <summary>
        ///     This event occurs when current match beguin
        /// </summary>
        public event MatchEvent MatchStarted;

        /// <summary>
        ///     This event occurs when current match is ended
        /// </summary>
        public event MatchEvent MatchEnded;

        /// <summary>
        ///     This event occurs when score of current match changed
        /// </summary>
        public event MatchEvent ScoreChanged;

        /// <summary>
        ///     This event occurs when score of current match is closed
        /// </summary>
        public event MatchEvent MatchClosed;

        #endregion
    }
}