using Contest.Domain;

namespace Contest.Service
{
    internal interface IUnitOfWorks
    {
        void Save(IEntity entity);

        void Begin();

        void Commit();

        void Rollback();
    }
}