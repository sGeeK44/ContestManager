using System;
using System.Collections.Generic;
using Contest.Core.Exceptions;

namespace Contest.Core.Repository
{
    public interface IDataContext<T> where T : class
    {
        /// <summary>
        /// Insert specified item from DataContext
        /// </summary>
        /// <param name="itemToInsert">Item to insert</param>
        /// <exception cref="Exceptions.ItemAlreadyExistException">Throw when item already exist into context</exception>
        void Insert(T itemToInsert);

        /// <summary>
        /// Update specified item from DataContext
        /// </summary>
        /// <param name="itemToUpdate">Item to update</param>
        /// <exception cref="NotFoundException">Throw when item doesn't exist into context</exception>
        void Update(T itemToUpdate);

        /// <summary>
        /// Delete specified item from DataContext
        /// </summary>
        /// <param name="itemToDelete">Item to update</param>
        /// <exception cref="NotFoundException">Throw when item doesn't exist into context</exception>
        void Delete(T itemToDelete);

        /// <summary>
        /// Return item list from context in according with specified predicate
        /// </summary>
        /// <param name="predicate">Predicate for search</param>
        /// <returns>Item List founded</returns>
        IList<T> Find(Func<T, bool> predicate);

        /// <summary>
        /// Return first item from context in according with specified predicate if exist else default(T)
        /// </summary>
        /// <param name="predicate">Predicate for search</param>
        /// <returns>Item founded or default(T)</returns>
        T FirstOrDefault(Func<T, bool> predicate);

        /// <summary>
        /// Determine existance of specified item
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>True if context contains item, false else</returns>
        bool IsExist(T item);

        /// <summary>
        /// Get number of item contains in context
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clear context
        /// </summary>
        void Clear();
    }
}