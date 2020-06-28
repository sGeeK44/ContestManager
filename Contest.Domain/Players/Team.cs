﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Players
{
    /// <summary>
    ///     Represent a team involved in a tournament
    /// </summary>
    [Serializable]
    [Entity(NameInStore = "TEAM")]
    public class Team : Entity, ITeam
    {
        #region Fields

        private Lazy<IContest> _contest;
        private Lazy<IList<IPerson>> _members;
        private Lazy<IList<IMatch>> _matchList;
        private Lazy<IList<IRelationship<ITeam, IGameStep>>> _gameStepTeamRelationshipList;
        private Lazy<IList<IRelationship<ITeam, IPhase>>> _phaseTeamRelationshipList;

        #endregion

        #region MEF Import

        [Import] private IRepository<IPerson> PersonRepository { get; set; }

        [Import] private IRepository<ITeam> TeamRepository { get; set; }

        [Import] private IRepository<IMatch> MatchRepository { get; set; }

        [Import] private IRepository<IContest> ContestRepository { get; set; }

        [Import] private IRepository<IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import] private IRepository<IRelationship<ITeam, IPhase>> TeamPhaseRelationshipRepository { get; set; }

        [Import] private IRelationshipFactory<ITeam, IGameStep> TeamGameStepRelationshipFactory { get; set; }

        [Import] private IRelationshipFactory<ITeam, IPhase> TeamPhaseRelationshipFactory { get; set; }

        #endregion

        #region Constructors

        private Team()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _members = new Lazy<IList<IPerson>>(() =>
                PersonRepository.Find(_ => _.AffectedTeamId == Id).ToList());
            _matchList = new Lazy<IList<IMatch>>(() => MatchRepository.Find(_ => _.Team1Id == Id)
                .Union(MatchRepository.Find(_ => _.Team2Id == Id))
                .ToList());
            _contest = new Lazy<IContest>(() =>
                ContestRepository.Find(_ => _.Id == ContestId).FirstOrDefault());
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() =>
                TeamGameStepRelationshipRepository
                    .Find(_ => _.FirstItemInvolveId == Id).ToList());
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() =>
                TeamPhaseRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id)
                    .ToList());
        }

        public Team(IContest contest, string name)
        {
            Id = Guid.NewGuid();
            Contest = contest;
            Name = name;
            Members = new List<IPerson>();
            MatchList = new List<IMatch>();
            _gameStepTeamRelationshipList =
                new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() => new List<IRelationship<ITeam, IGameStep>>());
            _phaseTeamRelationshipList =
                new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => new List<IRelationship<ITeam, IPhase>>());
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get name of current team
        /// </summary>
        [Field(FieldName = "NAME")]
        public string Name { get; set; }

        /// <summary>
        ///     Get all person who compose team
        /// </summary>
        public IList<IPerson> Members
        {
            get => _members.Value;
            set { _members = new Lazy<IList<IPerson>>(() => value); }
        }

        public IList<IMatch> MatchList
        {
            get => _matchList.Value;
            set { _matchList = new Lazy<IList<IMatch>>(() => value); }
        }

        /// <summary>
        ///     Get current match where team in involve
        /// </summary>
        public IMatch CurrentMatch
        {
            get { return MatchList.FirstOrDefault(_ => _.MatchState == MatchState.InProgress); }
        }

        /// <summary>
        ///     Get contest id associated to this team
        /// </summary>
        [Field(FieldName = "CONTEST_ID")]
        public Guid ContestId { get; private set; }

        /// <summary>
        ///     Get contest associated to this team
        /// </summary>
        public IContest Contest
        {
            get => _contest.Value;
            set
            {
                _contest = new Lazy<IContest>(() => value);
                ContestId = value?.Id ?? Guid.Empty;
            }
        }

        /// <summary>
        ///     Get GameStep involved in current team
        /// </summary>
        public IList<IGameStep> GameStepList
        {
            get { return _gameStepTeamRelationshipList.Value.Select(_ => _.SecondItemInvolve).ToList(); }
            set
            {
                _gameStepTeamRelationshipList.Value.Clear();
                foreach (var gamestep in value)
                    _gameStepTeamRelationshipList.Value.Add(TeamGameStepRelationshipFactory.Create(this, gamestep));
            }
        }

        /// <summary>
        ///     Get Phase involved in current team
        /// </summary>
        public IList<IPhase> PhaseList
        {
            get { return _phaseTeamRelationshipList.Value.Select(_ => _.SecondItemInvolve).ToList(); }
            set
            {
                _phaseTeamRelationshipList.Value.Clear();
                foreach (var phase in value)
                    _phaseTeamRelationshipList.Value.Add(TeamPhaseRelationshipFactory.Create(this, phase));
            }
        }

        #endregion

        #region Methods

        public void AddPlayer(IPerson playerToAdd)
        {
            if (playerToAdd == null) throw new ArgumentNullException(nameof(playerToAdd));
            if (Members.Count(item => item == playerToAdd) != 0) return;

            Members.Add(playerToAdd);
            playerToAdd.AffectedTeam = this;
        }

        public void RemovePlayer(IPerson playerToRemove)
        {
            if (playerToRemove == null) throw new ArgumentNullException(nameof(playerToRemove));
            Members.Remove(playerToRemove);
            playerToRemove.AffectedTeam = null;
        }

        public void AddMatch(IMatch match)
        {
            if (match == null) return;
            if (!MatchList.Contains(match)) MatchList.Add(match);
        }

        #endregion

        #region Factory

        #endregion
    }
}