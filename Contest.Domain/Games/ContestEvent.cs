namespace Contest.Domain.Games
{
    /// <summary>
    ///     Occurs when new phase start in a contest
    /// </summary>
    /// <param name="sender">Contest who throw event</param>
    /// <param name="newPhase">New phase started</param>
    public delegate void NextPhaseStartedEvent(IContest sender, IPhase newPhase);

    /// <summary>
    ///     Occurs by a contest
    /// </summary>
    /// <param name="sender">Contest who throw event</param>
    public delegate void ContestEvent(IContest sender);
}