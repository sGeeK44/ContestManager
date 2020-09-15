using System.Collections.Generic;
using Contest.Domain.Games;
using Contest.Domain.Matchs;

namespace Contest.Domain.Players
{
    public interface ITeam : IEntity
    {
        /// <summary>
        ///     Get name of current team
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Get current match where team in involve
        /// </summary>
        IMatch CurrentMatch { get; }

        /// <summary>
        ///     Get contest associated to this team
        /// </summary>
        IContest Contest { get; }

        /// <summary>
        ///     Get contest associated to this team
        /// </summary>
        IList<IRelationship<ITeam, IPerson>> Members { get; }

        /// <summary>
        /// Add team in a new match
        /// </summary>
        /// <param name="match">New match for current tem</param>
        void AddMatch(IMatch match);

        /// <summary>
        /// Add new person in current team
        /// </summary>
        /// <param name="person">New person involve in team</param>
        IRelationship<ITeam, IPerson> AddPlayer(IPerson person);

        /// <summary>
        /// Remove specified person from current team
        /// </summary>
        /// <param name="person">Person to remove</param>
        IRelationship<ITeam, IPerson> RemovePlayer(IPerson person);
    }
}