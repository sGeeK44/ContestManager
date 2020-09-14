using System;
using Contest.Domain.Games;
using Contest.Domain.Players;
using Contest.Domain.Settings;

namespace Contest.Domain.Matchs
{
    public interface IMatch : IEntity
    {
        /// <summary>
        ///     Get or Set first involved team in current match
        /// </summary>
        ITeam Team1 { get; set; }

        /// <summary>
        ///     Get first team match score
        /// </summary>
        ushort TeamScore1 { get; }

        /// <summary>
        ///     Get or Set second involved team in current match
        /// </summary>
        ITeam Team2 { get; set; }

        /// <summary>
        ///     Get second team match score
        /// </summary>
        ushort TeamScore2 { get; }

        /// <summary>
        ///     Get place where match is played
        /// </summary>
        IField MatchField { get; }

        /// <summary>
        ///     Get Time of beginning match
        /// </summary>
        DateTime? Beginning { get; }

        /// <summary>
        ///     Get Time between start and now
        /// </summary>
        TimeSpan? Elapse { get; }

        /// <summary>
        ///     Get Time of end match
        /// </summary>
        DateTime? Endded { get; }

        /// <summary>
        ///     Get winner of current match
        /// </summary>
        ITeam Winner { get; }

        /// <summary>
        ///     Get boolean to know if current match is beginning
        /// </summary>
        bool IsBeginning { get; }

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        ///     Indicate if current match is closed
        /// </summary>
        bool IsClose { get; }

        /// <summary>
        ///     Get boolean to know if current match is ended
        /// </summary>
        MatchState MatchState { get; }

        /// <summary>
        ///     Get current match setting
        /// </summary>
        IMatchSetting Setting { get; }

        /// <summary>
        ///     Start current match on specfied field
        /// </summary>
        /// <param name="field"></param>
        void Start(IField field);

        /// <summary>
        ///     Set result for current match.
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        void SetResult(ushort teamScore1, ushort teamScore2);

        /// <summary>
        ///     Update score of current match by specified value
        /// </summary>
        /// <param name="teamScore1">Point of first team</param>
        /// <param name="teamScore2">Point of second team</param>
        void UpdateScore(ushort teamScore1, ushort teamScore2);

        /// <summary>
        ///     Close current match
        /// </summary>
        void Close();

        /// <summary>
        ///     Get boolean to know if specified team is involve in current match
        /// </summary>
        /// <param name="team">Team to test</param>
        /// <returns>True if specified team is involved, false else.</returns>
        bool IsTeamInvolved(ITeam team);

        /// <summary>
        ///     This event occurs when current match beguin
        /// </summary>
        event MatchEvent MatchStarted;

        /// <summary>
        ///     This event occurs when current match is ended
        /// </summary>
        event MatchEvent MatchEnded;

        /// <summary>
        ///     This event occurs when score of current match changed
        /// </summary>
        event MatchEvent ScoreChanged;

        /// <summary>
        ///     This event occurs when score of current match is closed
        /// </summary>
        event MatchEvent MatchClosed;
    }
}