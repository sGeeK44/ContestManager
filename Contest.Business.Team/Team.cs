using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a team involved in a tournament
    /// </summary>
    [Serializable]
    [DataContract(Name = "TEAM")]
    public class Team : Identifiable<Team>, ITeam
    {
        #region Fields

        private Lazy<IContest> _contest;
        private Lazy<IList<IPerson>> _members;
        private Lazy<IList<IMatch>> _matchList;
        private Lazy<IList<IRelationship<ITeam, IGameStep>>> _gameStepTeamRelationshipList;
        private Lazy<IList<IRelationship<ITeam, IPhase>>> _phaseTeamRelationshipList;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IPerson> PersonRepository { get; set; }

        [Import]
        private IRepository<ITeam> TeamRepository { get; set; }

        [Import]
        private IRepository<IMatch> MatchRepository { get; set; }

        [Import]
        private IRepository<IContest> ContestRepository { get; set; }

        [Import]
        private IRepository<IRelationship<ITeam, IGameStep>> TeamGameStepRelationshipRepository { get; set; }

        [Import]
        private IRepository<IRelationship<ITeam, IPhase>> TeamPhaseRelationshipRepository { get; set; }

        [Import]
        private IRelationshipFactory<ITeam, IGameStep> TeamGameStepRelationshipFactory { get; set; }

        [Import]
        private IRelationshipFactory<ITeam, IPhase> TeamPhaseRelationshipFactory { get; set; }

        #endregion

        #region Constructors

        private Team()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _members = new Lazy<IList<IPerson>>(() => PersonRepository.Find(_ => _.AffectedTeamId == Id).ToList());
            _matchList = new Lazy<IList<IMatch>>(() => MatchRepository.Find(_ => _.Team1Id == Id)
                                                                      .Union(MatchRepository.Find(_ => _.Team2Id == Id))
                                                                      .ToList());
            _contest = new Lazy<IContest>(() => ContestRepository.Find(_ => _.Id == ContestId).FirstOrDefault());
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() => TeamGameStepRelationshipRepository.Find(_ => _.FirstItemInvolveId == Id));
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => TeamPhaseRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id));
        }

        public Team(IContest contest, string name)
        {
            Id = Guid.NewGuid();
            Contest = contest;
            Name = name;
            Members = new List<IPerson>();
            MatchList = new List<IMatch>();
            _gameStepTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IGameStep>>>(() => new List<IRelationship<ITeam, IGameStep>>());
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => new List<IRelationship<ITeam, IPhase>>());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get name of current team
        /// </summary>
        [SqlField(Name = "NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Get all person who compose team
        /// </summary>
        public IList<IPerson> Members
        {
            get { return _members.Value; }
            set { _members = new Lazy<IList<IPerson>>(() => value); }
        }

        public IList<IMatch> MatchList
        {
            get { return _matchList.Value; }
            set { _matchList = new Lazy<IList<IMatch>>(() => value); }
        }

        /// <summary>
        /// Get current match where team in involve
        /// </summary>
        public IMatch CurrentMatch
        {
            get
            {
                return MatchList.FirstOrDefault(_ => _.MatchState == MatchState.InProgress);
            }
        }

        /// <summary>
        /// Get contest id associated to this team
        /// </summary>
        [SqlField(Name = "CONTEST_ID")]
        public Guid ContestId { get; private set; }

        /// <summary>
        /// Get contest associated to this team
        /// </summary>
        public IContest Contest
        {
            get
            {
                return _contest.Value;
            }
            set
            {
                _contest = new Lazy<IContest>(() => value);
                ContestId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get GameStep involved in current team
        /// </summary>
        public IList<IGameStep> GameStepList
        {
            get { return _gameStepTeamRelationshipList.Value.Select(_ => _.SecondItemInvolve).ToList(); }
            set
            {
                _gameStepTeamRelationshipList.Value.Clear();
                foreach (var gamestep in value)
                {
                    _gameStepTeamRelationshipList.Value.Add(TeamGameStepRelationshipFactory.Create(this, gamestep));
                }
            }
        }

        /// <summary>
        /// Get Phase involved in current team
        /// </summary>
        public IList<IPhase> PhaseList
        {
            get { return _phaseTeamRelationshipList.Value.Select(_ => _.SecondItemInvolve).ToList(); }
            set
            {
                _phaseTeamRelationshipList.Value.Clear();
                foreach (var phase in value)
                {
                    _phaseTeamRelationshipList.Value.Add(TeamPhaseRelationshipFactory.Create(this, phase));
                }
            }
        }

        #endregion

        #region Methods

        public void AddPlayer(IPerson playerToAdd)
        {
            if (playerToAdd == null) throw new ArgumentNullException("playerToAdd");
            if (Members.Count(item => item == playerToAdd) != 0) return;

            Members.Add(playerToAdd);
            playerToAdd.AffectedTeam = this;
        }

        public void RemovePlayer(IPerson playerToRemove)
        {
            if (playerToRemove == null) throw new ArgumentNullException("playerToRemove");
            Members.Remove(playerToRemove);
            playerToRemove.AffectedTeam = null;
        }

        public void AddMatch(IMatch match)
        {
            if (match == null) return;
            if (!MatchList.Contains(match)) MatchList.Add(match);
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<ITeam>(this);
            foreach (var member in Members)
            {
                member.PrepareCommit(unitOfWorks);
            }
        }

        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Factory

        #endregion
    }
}
