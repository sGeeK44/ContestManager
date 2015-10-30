namespace Contest.Business
{
    /// <summary>
    /// Represents the method that will handle Phase event
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    public delegate void PhaseEvent(object sender);

    /// <summary>
    /// Represents the method that will handle Phase event
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    public delegate void NextStepStartedEvent(object sender, IGameStep newGameStep);
}
