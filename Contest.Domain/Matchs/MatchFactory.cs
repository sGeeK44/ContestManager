using System;
using System.ComponentModel.Composition;
using Contest.Domain.Games;
using Contest.Domain.Players;

namespace Contest.Domain.Matchs
{
    [Export(typeof(IMatchFactory))]
    public class MatchFactory : IMatchFactory
    {
        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Match" /> with specified param
        /// </summary>
        /// <param name="team1">First team involved in match</param>
        /// <param name="team2">Second team involved in match</param>
        /// <param name="gameStep">Game step linked</param>
        /// <param name="setting">Set setting for new match</param>
        /// <returns>Match's instance</returns>
        public IMatch Create(IGameStep gameStep, ITeam team1, ITeam team2, IMatchSetting setting)
        {
            if (team1 == null) throw new ArgumentNullException(nameof(team1));
            if (team2 == null) throw new ArgumentNullException(nameof(team2));
            if (gameStep == null) throw new ArgumentNullException(nameof(gameStep));
            if (team1.Id == team2.Id)
                throw new ArgumentException("Les deux équipes d'un même match ne peuvent pas être identique.");
            if (setting == null) throw new ArgumentNullException(nameof(setting));

            IMatch result = new Match
            {
                Team1 = team1,
                Team2 = team2,
                TeamScore1 = 0,
                TeamScore2 = 0,
                MatchState = MatchState.Planned,
                GameStep = gameStep,
                Setting = setting
            };
            team1.AddMatch(result);
            team2.AddMatch(result);

            return result;
        }
    }
}