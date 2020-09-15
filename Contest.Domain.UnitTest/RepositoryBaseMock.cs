using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contest.Domain;
using SmartWay.Orm;
using SmartWay.Orm.Interfaces;
using IEntity = Contest.Domain.IEntity;

namespace Contest.Business.UnitTest
{
    public class RepositoryBaseMock<T, TI> : IRepository<T, TI> where T : IEntity, TI
    {
        private readonly List<T> _items = new List<T>();

        public void Save(TI entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TI entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(List<TI> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteByBundle(List<TI> entities, int bundleSize, IOrmObserver observer)
        {
            throw new NotImplementedException();
        }

        public TI GetByPk(object pk)
        {
            throw new NotImplementedException();
        }

        public List<TI> GetAllReference<TForeignEntity>(object fk)
        {
            throw new NotImplementedException();
        }

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

        public int Count()
        {
            throw new NotImplementedException();
        }

        public long CountAllReference<TForeignEntity>(object fk)
        {
            throw new NotImplementedException();
        }

        public IDataStore DataStore { get; set; }
    }
}