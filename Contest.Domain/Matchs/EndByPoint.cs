namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent a end by point constraint for a match
    /// </summary>
    public class EndByPoint : EndByBase
    {
        #region Constructors

        /// <summary>
        ///     Initialize a new instance of EndByPoint
        /// </summary>
        public EndByPoint(MatchSetting matchSetting)
            : base(matchSetting)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get type of End
        /// </summary>
        public override EndTypeConstaint EndBy => EndTypeConstaint.Point;

        #endregion

        #region Methods

        /// <summary>
        ///     Ensure match can be finished in according with current setting
        /// </summary>
        /// <param name="teamScore1">Team score 1</param>
        /// <param name="teamScore2">Team score 2</param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public override bool IsValidToFinishedMatch(ushort teamScore1, ushort teamScore2, out string message)
        {
            if (!IsValidScore(teamScore1, out message)) return false;
            if (!IsValidScore(teamScore2, out message)) return false;
            if (teamScore1 == MatchSetting.MatchPoint || teamScore2 == MatchSetting.MatchPoint) return true;
            message =
                $"Au moins une des deux équipes doit avoir un résultat égale un nombre du point du match. Résultat équipe 1:{teamScore1}. Résultat équipe 2:{teamScore2}. Match:{MatchSetting.MatchPoint}";
            return false;
        }

        /// <summary>
        ///     Ensure mscore is valid in according with current setting
        /// </summary>
        /// <param name="teamScore"></param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public override bool IsValidScore(ushort teamScore, out string message)
        {
            message = null;
            if (teamScore <= MatchSetting.MatchPoint) return true;

            message = $"Le résultat n'est pas valide. Résultat:{teamScore}. Valeur maximum:{MatchSetting.MatchPoint}.";
            return false;
        }

        /// <summary>
        ///     Boolean to indicate if type of end support match point
        /// </summary>
        public override bool CanUseMatchPoint => true;

        #endregion
    }
}