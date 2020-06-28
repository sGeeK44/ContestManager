namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent all kind of end type constraint for a MatchSetting
    /// </summary>
    public enum EndTypeConstaint
    {
        /// <summary>
        ///     Unknown type
        /// </summary>
        Unknown,

        /// <summary>
        ///     No specifiq constraint
        /// </summary>
        None,

        /// <summary>
        ///     MatchSetting can endded when a team have a specified score
        /// </summary>
        Point
    }
}