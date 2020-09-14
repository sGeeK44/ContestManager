using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Contest.Core.Component;
using Contest.Domain;
using SmartWay.Orm.Entity;
using SmartWay.Orm.Interfaces;
using SmartWay.Orm.Repositories;
using IEntity = Contest.Domain.IEntity;

namespace Contest.Repository
{
    public class SqlRepositoryBase<TEntity, TIEntity> : Repository<TEntity, TIEntity>, IRepository<TEntity, TIEntity>
        where TIEntity : class, IEntity, IDistinctableEntity
        where TEntity : class, TIEntity, new()
    {
        [Import]
        public override IDataStore DataStore { get; set; }

        public SqlRepositoryBase() { }

        public SqlRepositoryBase(IDataStore dataStore)
            : base(dataStore) { }

        public TIEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return DataStore.Select<TEntity>().Where(filter).GetValues().FirstOrDefault();
        }

        public IEnumerable<TIEntity> Find(Expression<Func<TEntity, bool>> filter)
        {
            return DataStore.Select<TEntity>().Where(filter).GetValues();
        }
    }
}
