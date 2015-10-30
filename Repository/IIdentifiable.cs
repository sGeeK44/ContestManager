using System;

namespace Contest.Core.Repository
{
    public interface IIdentifiable : IEquatable<IIdentifiable>
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid Id { get; }
    }
}
