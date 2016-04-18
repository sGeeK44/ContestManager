using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contest.Core.Repository
{
    /// <summary>
    /// Expose methods for CRUD action on T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Insert new item from repository
        /// </summary>
        /// <param name="item">New item to insert</param>
        void Insert(T item);
        
        /// <summary>
        /// Try to update before make an Insert of item from repository
        /// </summary>
        /// <param name="item">New or Existing item to insert/Update</param>
        void InsertOrUpdate(T item);
        
        /// <summary>
        /// Update existing item from repository
        /// </summary>
        /// <param name="item">Existing item to update</param>
        void Update(T item);

        /// <summary>
        /// Remove existing item from repository
        /// </summary>
        /// <param name="item">Old item to delete</param>
        void Delete(T item);

        /// <summary>
        /// Clear local cache
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Search item in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <retur>First item found or default(T)</retur>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Search items in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList<T> Find(Expression<Func<T, bool>> predicate);
    }
}
