using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contest.Core.Repository;
using Moq;

namespace Contest.UnitTest.TestKit
{
    public class RepositoryBaseMock<T> : IRepository<T>
        where T : class
    {
        public Mock<IRepository<T>> RepositoryMock { get; set; }

        public RepositoryBaseMock()
        {
            RepositoryMock = new Mock<IRepository<T>>();
        }

        public void ClearCache()
        {
            RepositoryMock.Object.ClearCache();
        }

        public void Delete(T item)
        {
            RepositoryMock.Object.Delete(item);
        }        

        public IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return RepositoryMock.Object.Find(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return RepositoryMock.Object.FirstOrDefault(predicate);
        }

        public void Insert(T item)
        {
            RepositoryMock.Object.Insert(item);
        }

        public void InsertOrUpdate(T item)
        {
            RepositoryMock.Object.InsertOrUpdate(item);
        }

        public void Update(T item)
        {
            RepositoryMock.Object.Update(item);
        }
    }
}