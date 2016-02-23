using System;
using System.Collections.Generic;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface ITeam : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get name of current team
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Get all person who compose team
        /// </summary>
        IList<IPerson> Members { get; }
        
        /// <summary>
        /// Get match list where current team is invovel
        /// </summary>
        IList<IMatch> MatchList { get; }

        /// <summary>
        /// Get current match where team in involve
        /// </summary>
        IMatch CurrentMatch { get; }

        /// <summary>
        /// Get contest id associated to this team
        /// </summary>
        Guid ContestId { get; }

        /// <summary>
        /// Get contest associated to this team
        /// </summary>
        IContest Contest { get; }

        /// <summary>
        /// Get GameStep involved in current team
        /// </summary>
        IList<IGameStep> GameStepList { get; set; }

        /// <summary>
        /// Get Phase involved in current team
        /// </summary>
        IList<IPhase> PhaseList { get; set; }

        void AddPlayer(IPerson playerToAdd);
        void RemovePlayer(IPerson playerToRemove);
        void AddMatch(IMatch match);
    }
}