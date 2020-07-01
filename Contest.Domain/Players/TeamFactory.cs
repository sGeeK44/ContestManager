using System;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Domain.Games;

namespace Contest.Domain.Players
{
    [Export(typeof(ITeamFactory))]
    public class TeamFactory : ITeamFactory
    {
        [Import] private IRepository<Team, ITeam> TeamRepository { get; set; }

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Team" /> with specified param
        /// </summary>
        /// <param name="contest">Contest in wich new team was involved</param>
        /// <param name="name">Name of new team</param>
        /// <returns>Team's instance</returns>
        public ITeam Create(IContest contest, string name)
        {
            if (contest == null) throw new ArgumentNullException(nameof(contest));
            var existingTeams = TeamRepository.Find(_ => _.ContestId == contest.Id && _.Name == name);
            if (existingTeams != null && existingTeams.Count() != 0)
                throw new ArgumentException("Le nom de l'équipe est déjà utilisé.");

            var result = new Team(contest, name);
            contest.Register(result);
            return result;
        }
    }
}