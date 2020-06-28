using System;
using Contest.Domain.Players;
using Contest.Domain.Settings;

namespace Contest.Domain.Matchs
{
    /// <summary>
    ///     Represent a Match beween two team
    /// </summary>
    public class PlannedMatchState : InternalMatchState
    {
        #region Constructors

        internal PlannedMatchState(Match match)
            : base(match)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get winner of current match
        /// </summary>
        public override ITeam Winner => null;

        /// <summary>
        ///     Get boolean to know if current match is beginning
        /// </summary>
        public override bool IsBeginning => false;

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        public override bool IsFinished => false;

        /// <summary>
        ///     Get boolean to know if current match is close
        /// </summary>
        public override bool IsClosed => false;

        /// <summary>
        ///     Get current state value
        /// </summary>
        public override MatchState State => MatchState.Planned;

        #endregion

        #region Methods

        /// <summary>
        ///     Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        public override void Start(IField field)
        {
            if (field == null) throw new ArgumentNullException(nameof(field));
            if (field.IsAllocated)
                throw new ArgumentException(
                    $"Le terrain spécifié pour le match est déjà occupé pour un match. Match:{field.MatchInProgess.Id}");
            if (CurrentMatch.Team1.CurrentMatch != null)
                throw new NotSupportedException(
                    $"L'équipe {CurrentMatch.Team1.Name} est déjà en train de disputer une partie.");
            if (CurrentMatch.Team2.CurrentMatch != null)
                throw new NotSupportedException(
                    $"L'équipe {CurrentMatch.Team2.Name} est déjà en train de disputer une partie.");

            CurrentMatch.MatchField = field;
            CurrentMatch.MatchField.Allocate(CurrentMatch);
            CurrentMatch.Beginning = DateTime.Now;
            CurrentMatch.CurrentState = new InProgressMatchState(CurrentMatch);
        }

        /// <summary>
        ///     Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public override bool SetResult(ushort teamScore1, ushort teamScore2)
        {
            throw new NotSupportedException("Match's result can not be set, because it was in planned state.");
        }

        /// <summary>
        ///     Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        /// <returns>True if current score was really updated, false else.</returns>
        public override bool UpdateScore(ushort teamScore1, ushort teamScore2)
        {
            throw new NotSupportedException("Match's score can not be updated, because it was in planned state.");
        }

        /// <summary>
        ///     Close current match. Score can't be updated after.
        /// </summary>
        /// <returns>True if State changed from a state to close</returns>
        public override void Close()
        {
            throw new NotSupportedException("Match can not be close, because it was in planned state.");
        }

        #endregion
    }
}