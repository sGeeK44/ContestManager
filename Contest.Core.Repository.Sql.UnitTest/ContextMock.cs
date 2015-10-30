using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public class ContextMock<T>: IDataContext<T> where T : class
    {
        public List<T> Item { get; set; }

        public ContextMock()
        {
            Item = new List<T>();
        }

        public void Insert(T itemToInsert)
        {
            Item.Add(itemToInsert);
            CountInsertCall++;
        }

        public void Update(T itemToUpdate)
        {
            Item.Remove(itemToUpdate);
            Item.Add(itemToUpdate);
            CountUpdateCall++;
        }

        public void Delete(T itemToDelete)
        {
            Item.Remove(itemToDelete);
            CountDeleteCall++;
        }

        public IList<T> Find(Func<T, bool> predicate)
        {
            return Item.Where(predicate).ToList();
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return Item.FirstOrDefault(predicate);
        }

        public bool IsExist(T item)
        {
            return Item.FirstOrDefault(_ => _ == item) != null;
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public int CountInsertCall { get; set; }
        public int CountUpdateCall { get; set; }
        public int CountDeleteCall { get; set; }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
