﻿using System;
using System.Collections;
using System.Linq.Expressions;

namespace Contest.Core.Repository
{
    /// <summary>
    /// Expose service to persist several object type with transaction mode.
    /// </summary>
    public interface IUnitOfWorks
    {
        /// <summary>
        /// Search items in datastore
        /// </summary>
        /// <param name="objectTypeSearch">Type of inner list element to search</param>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList Find(Type objectTypeSearch, LambdaExpression predicate);

        /// <summary>
        /// Search items in datacontext
        /// </summary>
        /// <param name="objectTypeSearch">Type of inner list element to search</param>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList FindInContext(Type objectTypeSearch, LambdaExpression predicate);

        /// <summary>
        /// Insert new item from repository
        /// </summary>
        /// <param name="item">New item to insert</param>
        void Insert<T>(T item) where T : class;

        /// <summary>
        /// Try to update before make an Insert of item from repository
        /// </summary>
        /// <param name="item">New or Existing item to insert/Update</param>
        void InsertOrUpdate<T>(T item) where T : class;

        /// <summary>
        /// Update existing item from repository
        /// </summary>
        /// <param name="item">Existing item to update</param>
        void Update<T>(T item) where T : class;

        /// <summary>
        /// Remove existing item from repository
        /// </summary>
        /// <param name="item">Old item to delete</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Persist all changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Undo all changes making after last commit
        /// </summary>
        void RollBack();
    }
}
