namespace Contest.Domain.Games
{
    public interface IGameSetting : IEntity
    {
        /// <summary>
        ///     Get minimum player register by team.
        /// </summary>
        uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        ///     Get maximum player register by team.
        /// </summary>
        uint MaximumPlayerByTeam { get; set; }
    }
}