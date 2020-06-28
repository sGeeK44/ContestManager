namespace Contest.Domain.Games
{
    public interface IEliminationStep : IGameStep
    {
        /// <summary>
        ///     Get type of current game step.
        /// </summary>
        EliminationType Type { get; }
    }
}