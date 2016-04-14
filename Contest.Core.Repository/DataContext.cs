using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.Exceptions;
using Contest.Core.Repository.Exceptions;

namespace Contest.Core.Repository
{
    /// <summary>
    /// Store items to build local cache. Default equality method is used.
    /// </summary>
    /// <typeparam name="T">Type of item to store</typeparam>
    public class DataContext<T> : IDataContext<T> where T : class, IQueryable
    {
        private readonly IList<T> _itemList = new List<T>();

        /// <summary>
        /// Insert specified item from DataContext
        /// </summary>
        /// <param name="itemToInsert">Item to insert</param>
        /// <exception cref="ItemAlreadyExistException">Throw when item already exist into context</exception>
        public void Insert(T itemToInsert)
        {
            if (itemToInsert == null) throw new ArgumentNullException("itemToInsert");
            var existingItem = SingleOrDefault(_itemList, itemToInsert);
            if (existingItem != null) throw new ItemAlreadyExistException();
            _itemList.Add(itemToInsert);
        }

        /// <summary>
        /// Update specified item from DataContext
        /// </summary>
        /// <param name="itemToUpdate">Item to update</param>
        /// <exception cref="NotFoundException">Throw when item doesn't exist into context</exception>
        public void Update(T itemToUpdate)
        {
            if (itemToUpdate == null) throw new ArgumentNullException("itemToUpdate");

            var existingItem = SingleOrDefault(_itemList, itemToUpdate);
            if (existingItem == null) throw new ItemNotFoundException();

            _itemList.Remove(existingItem);
            _itemList.Add(itemToUpdate);
        }

        /// <summary>
        /// Delete specified item from DataContext
        /// </summary>
        /// <param name="itemToDelete">Item to update</param>
        /// <exception cref="NotFoundException">Throw when item doesn't exist into context</exception>
        public void Delete(T itemToDelete)
        {
            var item = SingleOrDefault(_itemList, itemToDelete);
            if (item == null) throw new ItemNotFoundException();
            _itemList.Remove(item);
        }

        /// <summary>
        /// Return item list from context in according with specified predicate
        /// </summary>
        /// <param name="predicate">Predicate for search</param>
        /// <returns>Item List founded</returns>
        public IList<T> Find(Func<T, bool> predicate)
        {
            return _itemList.Where(predicate).ToList();
        }

        /// <summary>
        /// Return first item from context in according with specified predicate if exist else default(T)
        /// </summary>
        /// <param name="predicate">Predicate for search</param>
        /// <returns>Item founded or default(T)</returns>
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return _itemList.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Determine existance of specified item
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>True if context contains item, false else</returns>
        public bool IsExist(T item)
        {
            return SingleOrDefault(_itemList, item) != null;
        }

        /// <summary>
        /// Get number of item contains in context
        /// </summary>
        public int Count { get { return _itemList.Count; }}

        /// <summary>
        /// Clear context
        /// </summary>
        public void Clear()
        {
            _itemList.Clear();
        }

        private static T SingleOrDefault(IList<T> list, T itemSearch)
        {
            return itemSearch == null ? null : list.SingleOrDefault(itemSearch.AreSame);
        }
    }
}
