using System;

namespace Contest.Core.DataStore.Sql.ReferenceManyToMany
{
    public interface IIdentifiable
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid Id { get; }
    }
}
