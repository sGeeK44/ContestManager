using Contest.Core.Windows;

namespace Contest.Business
{
    /// <summary>
    /// Represent all avalaible elimination type
    /// </summary>
    public enum EliminationType : ushort
    {
        /// <summary>
        /// Final step
        /// </summary>
        [Display("Finale")]
        Final = 1,

        /// <summary>
        /// Semi final step
        /// </summary>
        [Display("Demi finale")]
        SemiFinal = 2,

        /// <summary>
        /// Quater final
        /// </summary>
        [Display("Quart de finale")]
        QuarterFinal = 4,

        /// <summary>
        /// Sicteenth round
        /// </summary>
        [Display("Huiti�me")]
        SixteenthRound = 8,

        /// <summary>
        /// Thirty second round
        /// </summary>
        [Display("Seizi�me")]
        ThirtySecondRound = 16,

        /// <summary>
        /// Sisty fourth round
        /// </summary>
        [Display("Trente-deuzi�me")]
        SixtyFourthRound = 32
    }
}