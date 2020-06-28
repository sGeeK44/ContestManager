using System;
using Contest.Domain.Players;
using Contest.Domain.Settings;

namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent a Match beween two team
    /// </summary>
    public abstract class InternalMatchState
    {
        #region Constructors

        protected InternalMatchState(Match match)
        {
            CurrentMatch = match ?? throw new ArgumentNullException(nameof(match));
        }

        #endregion

        #region Properties

        protected Match CurrentMatch { get; }

        /// <summary>
        ///     Get winner of current match
        /// </summary>
        public abstract ITeam Winner { get; }

        /// <summary>
        ///     Get boolean to know if current match is beginning
        /// </summary>
        public abstract bool IsBeginning { get; }

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        public abstract bool IsFinished { get; }

        /// <summary>
        ///     Get boolean to know if current match is close
        /// </summary>
        public abstract bool IsClosed { get; }

        /// <summary>
        ///     Get current state value
        /// </summary>
        public abstract MatchState State { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public abstract void Start(IField field);

        /// <summary>
        ///     Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public abstract bool SetResult(ushort teamScore1, ushort teamScore2);

        /// <summary>
        ///     Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public abstract bool UpdateScore(ushort teamScore1, ushort teamScore2);

        /// <summary>
        ///     Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <param name="assertInput">Boolean to indicate if assertion on input have to be done</param>
        /// <returns>True if current score was really updated, false else.</returns>
        protected bool UpdateScore(ushort teamScore1, ushort teamScore2, bool assertInput)
        {
            // Ensure have real update
            if (CurrentMatch.TeamScore1 == teamScore1 && CurrentMatch.TeamScore2 == teamScore2) return false;

            if (assertInput)
            {
                // Check input param
                if (!CurrentMatch.Setting.IsValidScore(teamScore1, out var message))
                    throw new ArgumentException(message);

                if (!CurrentMatch.Setting.IsValidScore(teamScore2, out message))
                    throw new ArgumentException(message);
            }

            // Update score only when all controls is fine
            CurrentMatch.TeamScore1 = teamScore1;
            CurrentMatch.TeamScore2 = teamScore2;
            return true;
        }

        /// <summary>
        ///     Close current match. Score can't be updated after.
        /// </summary>
        /// <returns>True if State changed from a state to close</returns>
        public abstract void Close();

        #endregion
    }
}