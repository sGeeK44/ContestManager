namespace Contest.Domain.Games
{
    public interface IGameSettingFactory
    {
        /// <summary>
        ///     Create a new instance of <see cref="T:Business.IGameSetting" /> with specified param
        /// </summary>
        /// <param name="minimumPlayerByTeam">Set minimum player register by team.</param>
        /// <param name="maximumPlayerByTeam">Set maximum player register by team.</param>
        /// <returns>GameSetting's instance</returns>
        IGameSetting Create(uint minimumPlayerByTeam, uint maximumPlayerByTeam);
    }
}