using Contest.Domain.Games;

namespace Contest.Domain.Players
{
    public interface ITeamFactory
    {
        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Team" /> with specified param
        /// </summary>
        /// <param name="contest">Contest in wich new team was involved</param>
        /// <param name="name">Name of new team</param>
        /// <returns>Team's instance</returns>
        ITeam Create(IContest contest, string name);
    }
}