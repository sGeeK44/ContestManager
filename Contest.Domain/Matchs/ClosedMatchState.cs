using System;

namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent a Match beween two team
    /// </summary>
    public class ClosedMatchState : FinishedMatchState
    {
        #region Constructors

        internal ClosedMatchState(Match match)
            : base(match)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get boolean to know if current match is close
        /// </summary>
        public override bool IsClosed => true;

        /// <summary>
        ///     Get current state value
        /// </summary>
        public override MatchState State => MatchState.Closed;

        #endregion

        #region Methods

        /// <summary>
        ///     Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public override bool UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            throw new NotSupportedException("Match can not be update, because it was in closed state.");
        }

        /// <summary>
        ///     Close current match. Score can't be updated after.
        /// </summary>
        public override void Close()
        {
            throw new NotSupportedException("Match can not be closed, because it was in closed state.");
        }

        #endregion
    }
}