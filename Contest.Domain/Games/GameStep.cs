﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

namespace Contest.Domain.Games
{
    public abstract class GameStep : Entity, IGameStep
    {
        #region Factories

        [Import] private IMatchFactory MatchFactory { get; set; }

        #endregion

        #region Fields

        private ReferenceHolder<IPhase, Guid> _phase;
        private ReferenceHolder<IMatchSetting, Guid> _currentMatchSetting;
        private Lazy<IList<IMatch>> _matchList;
        private Lazy<IList<IRelationship<ITeam, IGameStep>>> _gameStepTeamRelationshipList;

        #endregion

        #region MEF Import

        [Import] private IRepository<Phase, IPhase> PhaseRepository { get; set; }

        [Import] private IRepository<MatchSetting, IMatchSetting> MatchSettingRepository { get; set; }

        [Import] private IRepository<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import] private IRepository<Match, IMatch> MatchRepository { get; set; }

        [Import] private IRelationshipFactory<ITeam, IGameStep> RelationshipFactory { get; set; }

        #endregion

        #region Constructor

        protected GameStep()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _phase = new ReferenceHolder<IPhase, Guid>(PhaseRepository);
            _currentMatchSetting = new ReferenceHolder<IMatchSetting, Guid>(MatchSettingRepository);
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() =>
                TeamGameStepRelationshipRepository
                    .Find(_ => _.SecondItemInvolveId == Id).ToList());
            _matchList = new Lazy<IList<IMatch>>(() =>
            {
                IList<IMatch> result = MatchRepository.Find(_ => _.GameStepId == Id).ToList();
                foreach (var match in result) RegisterHandler(match);
                return result;
            });
        }

        protected GameStep(IPhase phase, IList<ITeam> teamList, IMatchSetting currentMatchSetting)
            : this()
        {
            if (teamList == null)
                throw new ArgumentNullException(nameof(teamList));

            FlippingContainer.Instance.ComposeParts(this);

            foreach (var team in teamList)
            {
                var count = teamList.Count(item => item == team);
                if (count != 1)
                    throw new ArgumentException(
                        $"La liste des équipes ne peut pas contenir plus deux fois la même équipe. Equipe:{team.Id}. Count:{count}.");
            }

            Phase = phase ?? throw new ArgumentNullException(nameof(phase));
            TeamList = teamList;
            CurrentMatchSetting = currentMatchSetting;
            MatchList = new List<IMatch>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get phase Id linked.
        /// </summary>
        [Field(FieldName = "PHASE_ID")]
        public Guid PhaseId
        {
            get => _phase.Id;
            private set => _phase.Id = value;
        }

        /// <summary>
        ///     Get phase linked.
        /// </summary>
        public IPhase Phase
        {
            get => _phase.Object;
            private set => _phase.Object = value;
        }

        /// <summary>
        ///     Get game step name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     Return next step, null if no futher step
        /// </summary>
        public abstract EliminationType? NextStep { get; }

        /// <summary>
        ///     Get setting game Id for current step
        /// </summary>
        [Field(FieldName = "CURRENT_MATCH_SETTING_ID")]
        public Guid CurrentMatchSettingId
        {
            get => _currentMatchSetting.Id;
            private set => _currentMatchSetting.Id = value;
        }

        /// <summary>
        ///     Get setting game for current step
        /// </summary>
        public IMatchSetting CurrentMatchSetting
        {
            get => _currentMatchSetting.Object;
            private set => _currentMatchSetting.Object = value;
        }

        /// <summary>
        ///     Get team involved in current game step.
        /// </summary>
        public IList<IRelationship<ITeam, IGameStep>> TeamGameStepList
        {
            get => _gameStepTeamRelationshipList.Value;
        }

        /// <summary>
        ///     Get team involved in current game step.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get { return TeamGameStepList.Select(_ => _.FirstItemInvolve).ToList(); }
            set
            {
                _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(
                    () => RelationshipFactory.CreateFromFirstItemList(value, this));
            }
        }

        /// <summary>
        ///     Get all match of current game step.
        /// </summary>
        public IList<IMatch> MatchList
        {
            get => _matchList.Value;
            protected set { _matchList = new Lazy<IList<IMatch>>(() => value); }
        }

        /// <summary>
        ///     Get boolean to know if current game step is finished.
        /// </summary>
        [Field(FieldName = "IS_FINISHED")]
        public bool IsFinished { get; private set; }

        /// <summary>
        ///     Get boolean to know if current game step is finished.
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
            foreach (var match in MatchList) match.Close();
            RaiseGameStepEvent(GameStepEnded);
        }

        #endregion

        #region Event

        protected void RaiseGameStepEvent(GameStepEvent @event)
        {
            @event?.Invoke(this);
        }

        public abstract IList<ITeam> GetDirectQualifiedTeam();

        /// <summary>
        ///     This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent GameStepEnded;

        /// <summary>
        ///     This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent MatchListComplete;

        /// <summary>
        ///     This event occurs when current game step is ended
        /// </summary>
        public event GameStepEvent RankChanged;

        #endregion
    }
}