using System.Collections.Generic;

namespace Contest.Business
{
    public interface IQualificationStep : IGameStep
    {
        /// <summary>
        /// Get number of qualification group
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Get boolean to know if match in current qualification step is played with revenge.
        /// </summary>
        bool MatchWithRevenge { get; }

        /// <summary>
        /// Get number of qualified team for current game step
        /// </summary>
        ushort NumberOfQualifiedTeam { get; }

        /// <summary>
        /// Compute actual rank of current game step
        /// </summary>
        IList<ITeam> Rank { get; }
    }
}