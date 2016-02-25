using System;

namespace Contest.Business
{
    /// <summary>
    /// Represent a Match beween two team
    /// </summary>
    internal class InProgressMatchState : InternalMatchState
    {
        #region Constructors

        internal InProgressMatchState(Match match)
            : base(match) { }

        #endregion

        #region Properties

        /// <summary>
        /// Get winner of current match
        /// </summary>
        public override ITeam Winner { get { return null; } }

        /// <summary>
        /// Get boolean to know if current match is beginning
        /// </summary>
        public override bool IsBeginning { get { return true; } }

        /// <summary>
        /// Get boolean to know if current match is ended
        /// </summary>
        public override bool IsFinished { get { return false; } }

        /// <summary>
        /// Get boolean to know if current match is close
        /// </summary>
        public override bool IsClosed { get { return false; } }

        /// <summary>
        /// Get current state value
        /// </summary>
        public override MatchState State { get { return MatchState.InProgress; } }

        #endregion

        #region Methods

        /// <summary>
        /// Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public override void Start(IField field)
        {
            throw new NotSupportedException("Match can not be state, because it was in progress state.");
        }

        /// <summary>
        /// Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public override bool SetResult(ushort teamScore1, ushort teamScore2)
        {
            string message;
            // Assert inner state with param.
            if (!CurrentMatch.Setting.IsValidToFinishedMatch(teamScore1, teamScore2, out message)) throw new ArgumentException(message);
            
            var result = UpdateScore(teamScore1, teamScore2, false);
            CurrentMatch.MatchField.Release();
            CurrentMatch.MatchField = null;
            CurrentMatch.Endded = DateTime.Now;
            CurrentMatch.CurrentState = new FinishedMatchState(CurrentMatch);
            return result;
        }

        /// <summary>
        /// Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public override bool UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            return UpdateScore(teamScore1, teamScore2, true);
        }

        /// <summary>
        /// Close current match. Score can't be updated after.
        /// </summary>
        /// <returns>True if State changed from a state to close</returns>
        public override void Close()
        {
            throw new NotSupportedException("Match can not be close, because it was in progress state.");
        }

        #endregion
    }
}
