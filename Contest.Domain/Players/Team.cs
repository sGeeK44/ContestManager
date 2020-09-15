using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Domain.Games;
using Contest.Domain.Matchs;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

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

        private ReferenceHolder<IContest, Guid> _contest;
        private Lazy<IList<IMatch>> _matchList;
        private Lazy<IList<IRelationship<ITeam, IPerson>>> _memberList;
        private Lazy<IList<IRelationship<ITeam, IGameStep>>> _gameStepTeamRelationshipList;
        private Lazy<IList<IRelationship<ITeam, IPhase>>> _phaseTeamRelationshipList;

        #endregion

        #region MEF Import

        [Import] private IRepository<Person, IPerson> PersonRepository { get; set; }

        [Import] private IRepository<Team, ITeam> TeamRepository { get; set; }

        [Import] private IRepository<Match, IMatch> MatchRepository { get; set; }

        [Import] private IRepository<Games.Contest, IContest> ContestRepository { get; set; }

        [Import] private IRepository<TeamPersonRelationship, IRelationship<ITeam, IPerson>> TeamPersonRelationshipRepository { get; set; }

        [Import] private IRepository<TeamGameStepRelationship, IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import] private IRepository<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> TeamPhaseRelationshipRepository { get; set; }

        [Import] private IRelationshipFactory<ITeam, IGameStep> TeamGameStepRelationshipFactory { get; set; }

        [Import] private IRelationshipFactory<ITeam, IPhase> TeamPhaseRelationshipFactory { get; set; }

        #endregion

        #region Constructors

        public Team()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _contest = new ReferenceHolder<IContest, Guid>(ContestRepository);
            _matchList = new Lazy<IList<IMatch>>(() => MatchRepository.Find(_ => _.Team1Id == Id)
                .Union(MatchRepository.Find(_ => _.Team2Id == Id))
                .ToList());
            _memberList = new Lazy<IList<IRelationship<ITeam, IPerson>>>(() => TeamPersonRelationshipRepository.Find(_ => _.FirstItemInvolveId == Id).ToList());
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() => TeamGameStepRelationshipRepository.Find(_ => _.FirstItemInvolveId == Id).ToList());
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => TeamPhaseRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id).ToList());
        }

        public Team(IContest contest, string name)
            : this()
        {
            Contest = contest;
            Name = name;
            MatchList = new List<IMatch>();
            Members = new List<IRelationship<ITeam, IPerson>>();
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

        public IList<IMatch> MatchList
        {
            get => _matchList.Value;
            set { _matchList = new Lazy<IList<IMatch>>(() => value); }
        }

        public IList<IRelationship<ITeam, IPerson>> Members
        {
            get => _memberList.Value;
            set { _memberList = new Lazy<IList<IRelationship<ITeam, IPerson>>>(() => value); }
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
        public Guid ContestId
        {
            get => _contest.Id;
            private set => _contest.Id = value;
        }

        /// <summary>
        ///     Get contest associated to this team
        /// </summary>
        public IContest Contest
        {
            get => _contest.Object;
            private set => _contest.Object = value;
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

        public void AddMatch(IMatch match)
        {
            if (match == null) return;
            if (!MatchList.Contains(match)) MatchList.Add(match);
        }

        public IRelationship<ITeam, IPerson> AddPlayer(IPerson person)
        {
            var association = new TeamPersonRelationship(this, person);
            _memberList.Value.Add(association);
            person.TeamList.Add(association);
            return association;
        }

        public IRelationship<ITeam, IPerson> RemovePlayer(IPerson person)
        {
            var association = Members.FirstOrDefault(_ => _.SecondItemInvolveId == person.Id);
            _memberList.Value.Remove(association);
            person.TeamList.Remove(association);
            return association;
        }

        #endregion

        #region Factory

        #endregion
    }
}