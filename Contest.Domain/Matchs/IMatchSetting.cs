namespace Contest.Domain.Matchs
{
    public interface IMatchSetting : IEntity
    {
        /// <summary>
        ///     Get type of End
        /// </summary>
        EndTypeConstaint EndBy { get; set; }

        /// <summary>
        ///     Get if match can be duce
        /// </summary>
        bool CanBeDuce { get; set; }

        /// <summary>
        ///     Get number of point need to end a match
        /// </summary>
        ushort MatchPoint { get; set; }

        /// <summary>
        ///     Get point earn when match is win
        /// </summary>
        ushort PointForWin { get; set; }

        /// <summary>
        ///     Get point earn when match is loosed
        /// </summary>
        ushort PointForLoose { get; set; }

        /// <summary>
        ///     Get point earn when match have no winner
        /// </summary>
        ushort PointForDuce { get; set; }

        /// <summary>
        ///     Ensure match can be finished in according with current setting
        /// </summary>
        /// <param name="teamScore1">Team score 1</param>
        /// <param name="teamScore2">Team score 2</param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        bool IsValidToFinishedMatch(ushort teamScore1, ushort teamScore2, out string message);

        /// <summary>
        ///     Ensure mscore is valid in according with current setting
        /// </summary>
        /// <param name="teamScore"></param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        bool IsValidScore(ushort teamScore, out string message);
    }
}