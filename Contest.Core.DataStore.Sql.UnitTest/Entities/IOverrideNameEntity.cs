using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.UnitTest.Entities
{
    public interface IOverrideNameEntity : IIdentifiable, IQueryable
    {
        string Name { get; set; }

        bool Active { get; set; }

        int Age { get; set; }
    }
}