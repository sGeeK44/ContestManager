using System;
using System.ComponentModel.Composition;
using SmartWay.Orm.Sql;
using IEntity = Contest.Domain.IEntity;

namespace Contest.Service
{
    [Export(typeof(IUnitOfWorks))]
    public class UnitOfWorks : IUnitOfWorks
    {
        [Import] private ISqlDataStore DataStore { get; set; }

        public void Save(IEntity entity)
        {
            if (entity.Id == Guid.Empty)
                DataStore.Insert(entity);
            else
                DataStore.Update(entity);
        }

        public void Delete(IEntity entity)
        {
            DataStore.Delete(entity);
        }

        public void Begin()
        {
            DataStore.BeginTransaction();
        }

        public void Commit()
        {
            DataStore.Commit();
        }

        public void Rollback()
        {
            DataStore.Rollback();
        }
    }
}