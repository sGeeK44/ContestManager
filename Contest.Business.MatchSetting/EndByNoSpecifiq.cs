namespace Contest.Business
{
    /// <summary>
    /// Represent a end by point constraint for a match
    /// </summary>
    public class EndByNoSpecifiq : EndByBase
    {

        #region Constructors

        /// <summary>
        /// Initialize a new instance of EndByPoint
        /// </summary>
        public EndByNoSpecifiq(MatchSetting matchSetting)
            : base(matchSetting)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Get type of End
        /// </summary>
        public override EndTypeConstaint EndBy
        {
            get { return EndTypeConstaint.None; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ensure match can be finished in according with current setting
        /// </summary>
        /// <param name="teamScore1">Team score 1</param>
        /// <param name="teamScore2">Team score 2</param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public override bool IsValidToFinishedMatch(ushort teamScore1, ushort teamScore2, out string message)
        {
            message = null;
            return true;
        }

        /// <summary>
        /// Ensure mscore is valid in according with current setting
        /// </summary>
        /// <param name="teamScore"></param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public override bool IsValidScore(ushort teamScore, out string message)
        {
            message = null;
            return true;
        }

        /// <summary>
        /// Boolean to indicate if type of end support match point
        /// </summary>
        public override bool CanUseMatchPoint
        {
            get { return false; }
        }

        #endregion
    }
}
