using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contest.Core.Repository.Sql;
using Moq;

namespace Contest.UnitTest.Kit
{
    public class SqlRepositoryBaseMock<T> : ISqlRepository<T>
        where T : class
    {
        public Mock<ISqlRepository<T>> RepositoryMock { get; set; }

        public IList Find(Type objectTypeSearch, LambdaExpression predicate)
        {
            throw new NotImplementedException();
        }

        ISqlUnitOfWorks ISqlRepository.UnitOfWorks { get; set; }

        public ISqlUnitOfWorks UnitOfWorks
        {
            get { return RepositoryMock.Object.UnitOfWorks; }
            set { RepositoryMock.Object.UnitOfWorks = value; }
        }

        public SqlRepositoryBaseMock()
        {
            RepositoryMock = new Mock<ISqlRepository<T>>();
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

        public void CreateTable()
        {
            RepositoryMock.Object.CreateTable();
        }
    }
}