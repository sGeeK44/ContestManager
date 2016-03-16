using System.Collections.Generic;

namespace Contest.Business
{
    public interface IEliminationStep : IGameStep
    {
        /// <summary>
        /// Get type of current game step.
        /// </summary>
        EliminationType Type { get; }
    }
}