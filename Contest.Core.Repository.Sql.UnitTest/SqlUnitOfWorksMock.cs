using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public class SqlUnitOfWorksMock : ISqlUnitOfWorks
    {
        public SqlUnitOfWorksMock()
        {
            Items = new List<object>();
            Requests = new List<string>();
        }

        public bool ConstainsRepository<T>(ISqlRepository<T> repository) where T : class
        {
            throw new NotImplementedException();
        }

        public void AddRepository<T>(ISqlRepository<T> repository) where T : class
        {
            throw new NotImplementedException();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public IList<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsBinded { get; private set; }

        public void Insert<T>(T item) where T : class
        {
            CountInsertCall++;
            Items.Add(item);
        }

        public IList<object> Items { get; set; }

        public void InsertOrUpdate<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
            CountUpdateCall++;
        }

        public void Delete<T>(T item) where T : class
        {
            CountDeleteCall++;
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void AddRequest(string request)
        {
            CountAddRequestCall++;
            Requests.Add(request);
        }

        public IList<string> Requests { get; set; }

        public int CountAddRequestCall { get; set; }
        public int CountInsertCall { get; set; }
        public int CountDeleteCall { get; set; }
        public int CountUpdateCall { get; set; }
    }
}
