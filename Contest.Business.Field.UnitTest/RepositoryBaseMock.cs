using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Contest.Business.UnitTest
{
    public class RepositoryBaseMock<T>
    {
        public List<T> Content { get; set; }
        public int ClearCacheCount { get; private set; }
        public int DeleteCount { get; private set; }
        public int InsertCount { get; private set; }
        public int InsertOrUpdateCount { get; private set; }
        public int UpdateCount { get; private set; }

        public RepositoryBaseMock()
        {
            Content = new List<T>();
        }

        public void ClearCache()
        {
            ClearCacheCount++;
        }

        public void Delete(T item)
        {
            Content.Remove(item);
            DeleteCount++;
        }        

        public IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Content.Where(predicate.Compile()).ToList();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Content.FirstOrDefault(predicate.Compile());
        }

        public void Insert(T item)
        {
            Content.Add(item);
            InsertCount++;
        }

        public void InsertOrUpdate(T item)
        {
            Content.Remove(item);
            Content.Add(item);
            InsertOrUpdateCount++;
        }

        public void Update(T item)
        {
            Content.Remove(item);
            Content.Add(item);
            UpdateCount++;
        }
    }
}