using System;

namespace Contest.Core.Repository
{
    public interface IIdentifiable
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid Id { get; }
    }
}
