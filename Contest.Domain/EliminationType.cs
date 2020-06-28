namespace Contest.Domain
{
    /// <summary>
    ///     Represent all avalaible elimination type
    /// </summary>
    public enum EliminationType
    {
        /// <summary>
        ///     Final step
        /// </summary>
        Final = 1,

        /// <summary>
        ///     Semi final step
        /// </summary>
        SemiFinal = 2,

        /// <summary>
        ///     Quater final
        /// </summary>
        QuarterFinal = 4,

        /// <summary>
        ///     Sicteenth round
        /// </summary>
        SixteenthRound = 8,

        /// <summary>
        ///     Thirty second round
        /// </summary>
        ThirtySecondRound = 16,

        /// <summary>
        ///     Sisty fourth round
        /// </summary>
        SixtyFourthRound = 32
    }
}