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
        [Display("Huitième")]
        SixteenthRound = 8,

        /// <summary>
        /// Thirty second round
        /// </summary>
        [Display("Seizième")]
        ThirtySecondRound = 16,

        /// <summary>
        /// Sisty fourth round
        /// </summary>
        [Display("Trente-deuzième")]
        SixtyFourthRound = 32
    }
}