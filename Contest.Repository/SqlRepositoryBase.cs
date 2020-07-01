using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Contest.Domain;
using SmartWay.Orm.Filters;
using SmartWay.Orm.Interfaces;
using IEntity = Contest.Domain.IEntity;

namespace Contest.Repository
{
    public class SqlRepositoryBase<TEntity, TIEntity> : IRepository<TEntity, TIEntity> where TIEntity : class, IEntity where TEntity : class, TIEntity

    {
        [Import]
        protected IDataStore DataStore { get; set; }

        public SqlRepositoryBase()
        {
        }

        public SqlRepositoryBase(IDataStore dataStore)
        {
            DataStore = dataStore;
        }

        /// <summary>
        ///     Save specified entity from repository
        /// </summary>
        /// <param name="entity">Entity to save</param>
        public virtual void Save(TIEntity entity)
        {
            if (entity == null)
                return;

            if (entity.Id == Guid.Empty
                || GetByGuid(entity.Id) == null)
                DataStore.Insert(entity);
            else
                DataStore.Update(entity);
        }

        /// <summary>
        ///     Delete specifie specified entity from repository
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        public virtual void Delete(TIEntity entity)
        {
            if (entity == null)
                return;

            DataStore.Delete(entity);
        }

        public TIEntity GetByGuid(Guid guid)
        {
            var condition =
                DataStore.Condition<TEntity>(Entity.IdColumnName, guid, FilterOperator.Equals);
            return DataStore.Select<TEntity, TIEntity>().Where(condition).GetValues().FirstOrDefault();
        }

        /// <summary>
        ///     Search all entity in database
        /// </summary>
        /// <returns>All Entity found or empty list</returns>
        public virtual List<TIEntity> GetAll()
        {
            return DataStore.Select<TEntity, TIEntity>().GetValues().ToList();
        }

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
