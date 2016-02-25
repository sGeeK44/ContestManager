using System;

namespace Contest.Business
{
    /// <summary>
    /// Represent a Match beween two team
    /// </summary>
    internal class ClosedMatchState : InternalMatchState
    {
        #region Constructors

        internal ClosedMatchState(Match match)
            : base(match) { }

        #endregion

        #region Properties

        /// <summary>
        /// Get winner of current match
        /// </summary>
        public override ITeam Winner
        {
            get
            {

                if (CurrentMatch.TeamScore1 > CurrentMatch.TeamScore2) return CurrentMatch.Team1;
                if (CurrentMatch.TeamScore2 > CurrentMatch.TeamScore1) return CurrentMatch.Team2;
                return null;
            } 
        }

        /// <summary>
        /// Get boolean to know if current match is beginning
        /// </summary>
        public override bool IsBeginning { get { return true; } }

        /// <summary>
        /// Get boolean to know if current match is ended
        /// </summary>
        public override bool IsFinished { get { return true; } }

        /// <summary>
        /// Get boolean to know if current match is close
        /// </summary>
        public override bool IsClosed { get { return true; } }

        /// <summary>
        /// Get current state value
        /// </summary>
        public override MatchState State { get { return MatchState.Closed; } }

        #endregion

        #region Methods

        /// <summary>
        /// Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public override void Start(IField field)
        {
            throw new NotSupportedException("Match can not be start, because it was in closed state.");
        }

        /// <summary>
        /// Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public override bool SetResult(ushort teamScore1, ushort teamScore2)
        {
            throw new NotSupportedException("Match can not be set, because it was in closed state.");
        }

        /// <summary>
        /// Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        public override bool UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            throw new NotSupportedException("Match can not be update, because it was in closed state.");
        }

        /// <summary>
        /// Close current match. Score can't be updated after.
        /// </summary>
        public override void Close()
        {
            throw new NotSupportedException("Match can not be closed, because it was in closed state.");
        }

        #endregion
    }
}
