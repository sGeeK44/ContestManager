using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public abstract class GameStep : Identifiable<GameStep>, IGameStep
    {
        #region Fields

        private Lazy<IPhase> _phase;
        private Lazy<IMatchSetting> _currentMatchSetting;
        private Lazy<IList<IMatch>> _matchList;
        private Lazy<IList<IRelationship<ITeam, IGameStep>>> _gameStepTeamRelationshipList;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IPhase> PhaseRepository { get; set; }

        [Import]
        private IRepository<IMatchSetting> MatchSettingRepository { get; set; }

        [Import]
        private IRepository<IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import]
        private IRepository<IMatch> MatchRepository { get; set; }

        [Import]
        private IRelationshipFactory<ITeam, IGameStep> RelationshipFactory { get; set; }

        #endregion

        #region Constructor

        protected GameStep()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _phase = new Lazy<IPhase>(() => PhaseRepository.FirstOrDefault(_ => _.Id == PhaseId));
            _currentMatchSetting = new Lazy<IMatchSetting>(() => MatchSettingRepository.FirstOrDefault(_ => _.Id == CurrentMatchSettingId));
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() => TeamGameStepRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id));
            _matchList = new Lazy<IList<IMatch>>(() =>
            {
                IList<IMatch> result = MatchRepository.Find(_ => _.GameStepId == Id).ToList();
                foreach (var match in result) RegisterHandler(match);
                return result;
            });
        }

        protected GameStep(IPhase phase, IList<ITeam> teamList, IMatchSetting currentMatchSetting)
        {
            if (phase == null) throw new ArgumentNullException("phase");
            if (teamList == null) throw new ArgumentNullException("teamList");
            FlippingContainer.Instance.ComposeParts(this);
            
            foreach (var team in teamList)
            {
                var count = teamList.Count(item => item == team);
                if (count != 1) throw new ArgumentException(string.Format("La liste des équipes ne peut pas contenir plus deux fois la même équipe. Equipe:{0}. Count:{1}.", team.Id, count));
            }

            Phase = phase;
            TeamList = teamList;
            CurrentMatchSetting = currentMatchSetting;
            MatchList = new List<IMatch>();
        }

        #endregion

        #region Factories

        [Import]
        private IMatchFactory MatchFactory { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Get phase Id linked.
        /// </summary>
        [SqlField(Name = "PHASE_ID")]
        public Guid PhaseId { get; private set; }

        /// <summary>
        /// Get phase linked.
        /// </summary>
        public IPhase Phase
        {
            get { return _phase.Value; }
            private set
            {
                _phase = new Lazy<IPhase>(() => value);
                PhaseId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get game step name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Return next step, null if no futher step
        /// </summary>
        public abstract EliminationType? NextStep { get; }

        /// <summary>
        /// Get setting game Id for current step
        /// </summary>
        [SqlField(Name = "CURRENT_MATCH_SETTING_ID")]
        public Guid CurrentMatchSettingId { get; protected set; }

        /// <summary>
        /// Get setting game for current step
        /// </summary>
        public IMatchSetting CurrentMatchSetting
        {
            get { return _currentMatchSetting.Value; }
            protected set
            {
                _currentMatchSetting = new Lazy<IMatchSetting>(() => value);
                CurrentMatchSettingId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get team involved in current game step.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get { return _gameStepTeamRelationshipList.Value.Select(_ => _.FirstItemInvolve).ToList(); }
            set
            {
                _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(
                        () => RelationshipFactory.CreateFromFirstItemList(value, this));
            }
        }

        /// <summary>
        /// Get all match of current game step.
        /// </summary>
        public IList<IMatch> MatchList
        {
            get { return _matchList.Value; } 
            protected set { _matchList = new Lazy<IList<IMatch>>(() => value); } 
        }

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        [SqlField(Name = "IS_FINISHED")]
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        public bool IsMatchListComplete
        {
            get { return MatchList.Count(item => !item.IsFinished) == 0; }
        }

        #endregion

        #region Method

        public abstract void BuildMatch();

        protected void CreateMatch(ITeam t1, ITeam t2)
        {
            var match = MatchFactory.Create(this, t1, t2, CurrentMatchSetting);
            MatchList.Add(match);
            RegisterHandler(match);
        }

        private void RegisterHandler(IMatch match)
        {
            match.ScoreChanged += sender => RaiseGameStepEvent(RankChanged);
            match.MatchEnded += sender =>
            {
                if (IsMatchListComplete) RaiseGameStepEvent(MatchListComplete);
            };
        }

        public void EndGameStep()
        {
            if (!IsMatchListComplete) throw new NotSupportedException("Tous les matchs de l'étape ne sont pas fini.");
            IsFinished = true;
            foreach (var match in MatchList)
            {
                match.Close();
            }
            RaiseGameStepEvent(GameStepEnded);
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            foreach (var match in MatchList)
            {
                match.PrepareCommit(unitOfWorks);
            }
            if (_gameStepTeamRelationshipList.Value == null) return;

            // TODO Save de la table des relations
            foreach (var team in _gameStepTeamRelationshipList.Value.Select(_ => _.FirstItemInvolve))
            {
                team.PrepareCommit(unitOfWorks);
            }
        }

        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Event
        
        protected void RaiseGameStepEvent(GameStepEvent @event)
        {
            if (@event != null) @event(this);
        }

        public abstract IList<ITeam> GetDirectQualifiedTeam();

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent GameStepEnded;

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent MatchListComplete;

        /// <summary>
        /// This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent RankChanged;

        #endregion
    }
}
