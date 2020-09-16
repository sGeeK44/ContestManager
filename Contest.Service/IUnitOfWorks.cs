using Contest.Domain;
using Contest.Domain.Players;

namespace Contest.Service
{
    public interface IUnitOfWorks
    {
        void Save(IEntity entity);

        void Delete(IEntity entity);

        void Begin();

        void Commit();

        void Rollback();
    }
}