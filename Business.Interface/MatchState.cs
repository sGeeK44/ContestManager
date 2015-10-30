namespace Contest.Business
{
    /// <summary>
    /// Represent all state for a match
    /// </summary>
    public enum MatchState
    {
        /// <summary>
        /// Match is planned
        /// </summary>
        Planned,

        /// <summary>
        /// Match is in progress
        /// </summary>
        InProgress,

        /// <summary>
        /// Match is ended
        /// </summary>
        Finished,

        /// <summary>
        /// Match is closed
        /// </summary>
        Closed
    }
}
