using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contest.Core.Repository;

namespace Contest.Business.UnitTest
{
    public class RepositoryMockBase<T>
    {
        private readonly IList<T> _context = new List<T>();

        public void CreateTable()
        {
            throw new NotImplementedException();
        }

        public void Insert(T item)
        {
            _context.Add(item);
        }

        public void InsertOrUpdate(T item)
        {
            _context.Remove(item);
            _context.Add(item);
        }

        public void Update(T item)
        {
            _context.Remove(item);
            _context.Add(item);
        }

        public void Delete(T item)
        {
            _context.Remove(item);
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _context.FirstOrDefault(predicate.Compile());
        }

        public IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Where(predicate.Compile()).ToList();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWorks UnitOfWorks { get; set; }
    }
}
