﻿using System;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a relation n to n between two object
    /// </summary>
    public interface IRelationship<TIObj1, TIObj2> : IQueryable, ISqlPersistable
        where TIObj1 : IIdentifiable
        where TIObj2 : IIdentifiable
    {
        /// <summary>
        /// Get Id of first object involved
        /// </summary>
        Guid FirstItemInvolveId { get; }

        /// <summary>
        /// Get first object involved
        /// </summary>
        TIObj1 FirstItemInvolve { get; set; }

        /// <summary>
        /// Get Id of second object involved
        /// </summary>
        Guid SecondItemInvolveId { get; }

        /// <summary>
        /// Get second object involved
        /// </summary>
        TIObj2 SecondItemInvolve { get; set; }
    }
}