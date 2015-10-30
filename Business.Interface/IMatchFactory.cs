namespace Contest.Business
{
    public interface IMatchFactory
    {
        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Match"/> with specified param
        /// </summary>
        /// <param name="team1">First team involved in match</param>
        /// <param name="team2">Second team involved in match</param>
        /// <param name="gameStep">Game step linked</param>
        /// <param name="setting">Set setting for new match</param>
        /// <returns>Match's instance</returns>
        IMatch Create(IGameStep gameStep, ITeam team1, ITeam team2, IMatchSetting setting);
    }
}