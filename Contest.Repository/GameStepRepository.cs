using System.Collections.Generic;
using System.ComponentModel.Composition;
using Contest.Domain.Games;
using SmartWay.Orm;
using SmartWay.Orm.Interfaces;
using SmartWay.Orm.Repositories;

namespace Contest.Repository
{
    [Export(typeof(IRepository<IGameStep>))]
    public class GameStepRepository : IRepository<IGameStep>
    {
        [Import] private IRepository<IEliminationStep> EliminationStep { get; set; }

        [Import] private IRepository<IQualificationStep> QualificationStep { get; set; }

        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public long CountAllReference<TForeignEntity>(object fk)
        {
            throw new System.NotImplementedException();
        }

        public IDataStore DataStore { get; set; }
        public void Save(IGameStep entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(IGameStep entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(List<IGameStep> entities)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteByBundle(List<IGameStep> entities, int bundleSize, IOrmObserver observer)
        {
            throw new System.NotImplementedException();
        }

        public IGameStep GetByPk(object pk)
        {
            return (IGameStep) EliminationStep.GetByPk(pk) ?? QualificationStep.GetByPk(pk);
        }

        public List<IGameStep> GetAllReference<TForeignEntity>(object fk)
        {
            throw new System.NotImplementedException();
        }

        public List<IGameStep> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}