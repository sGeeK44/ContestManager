using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<IField>))]
    public class FieldRepositoryMock : RepositoryBaseMock<IField>, IRepository<IField>
    {
    }

    public class RepositoryBaseMock<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new List<T>();

        public List<T> GetAll()
        {
            return _items;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return _items.FirstOrDefault(filter.Compile());
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter)
        {
            return _items.Where(filter.Compile());
        }
    }
}