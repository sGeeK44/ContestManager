using System;
using Contest.Domain.Games;
using Contest.Domain.Matchs;

namespace Contest.Domain.Settings
{
    public interface IField : IEntity
    {
        /// <summary>
        ///     Get name of current field
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Get boolean to know if current field is allocated to a match
        /// </summary>
        bool IsAllocated { get; }

        /// <summary>
        ///     Get current match id in progress on current field if it is allocated, else null.
        /// </summary>
        Guid MatchInProgessId { get; }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        IMatch MatchInProgess { get; }

        /// <summary>
        ///     Get current contest id
        /// </summary>
        Guid CurrentContestId { get; }

        /// <summary>
        ///     Get current match in progress on current field if it is allocated, else null.
        /// </summary>
        IContest CurrentContest { get; }

        /// <summary>
        ///     Allocate current field forspecified match
        /// </summary>
        /// <param name="match">Match for wich you allocated field</param>
        /// <exception cref="System.ArgumentNullException" />
        /// <exception cref="System.ArgumentException">Throw when current field is already allocated.</exception>
        void Allocate(IMatch match);

        /// <summary>
        ///     Release current field of allocated match.
        /// </summary>
        void Release();
    }
}