namespace Contest.Business
{
    public abstract class EndByBase
    {
        protected MatchSetting MatchSetting { get; set; }

        /// <summary>
        /// Initialize a new instance of EndByPoint
        /// </summary>
        protected EndByBase(MatchSetting matchSetting)
        {
            MatchSetting = matchSetting;
        }

        /// <summary>
        /// Get type of End
        /// </summary>
        public abstract EndTypeConstaint EndBy { get; }

        /// <summary>
        /// Ensure match can be finished in according with current setting
        /// </summary>
        /// <param name="teamScore1">Team score 1</param>
        /// <param name="teamScore2">Team score 2</param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public abstract bool IsValidToFinishedMatch(ushort teamScore1, ushort teamScore2, out string message);

        /// <summary>
        /// Ensure mscore is valid in according with current setting
        /// </summary>
        /// <param name="teamScore"></param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public abstract bool IsValidScore(ushort teamScore, out string message);

        /// <summary>
        /// Boolean to indicate if type of end support match point
        /// </summary>
        public abstract bool CanUseMatchPoint { get; }
    }
}