using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Contest.Domain;
using Contest.Domain.Settings;

namespace Contest.Business.UnitTest
{
    [Export(typeof(IRepository<Field, IField>))]
    public class FieldRepositoryMock : RepositoryBaseMock<Field, IField>
    {
    }

    public class RepositoryBaseMock<T, TI> : IRepository<T, TI> where T : IEntity, TI
    {
        private readonly List<T> _items = new List<T>();

        public List<TI> GetAll()
        {
            return _items.Cast<TI>().ToList();
        }

        public TI FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return (TI)_items.FirstOrDefault(filter.Compile());
        }

        public IEnumerable<TI> Find(Expression<Func<T, bool>> filter)
        {
            return _items.Where(filter.Compile()).Cast<TI>();
        }
    }
}