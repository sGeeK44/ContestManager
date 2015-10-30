using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql.UnitTest
{
    class SqlRepositoryMock<I> : ISqlRepository<I> where I : class 
    {
        private IList<I> _data = new List<I>();
        public void Add(I item)
        {
            _data.Add(item);
        }

        public I FirstOrDefault(Expression<Func<I, bool>> predicate)
        {
            return _data.FirstOrDefault(predicate.Compile());
        }

        public IList<I> Find(Expression<Func<I, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void CreateTable()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public IUnitOfWorks UnitOfWorks
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Insert(I item)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(I item)
        {
            throw new NotImplementedException();
        }

        public void Update(I item)
        {
            throw new NotImplementedException();
        }

        public void Delete(I item)
        {
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
