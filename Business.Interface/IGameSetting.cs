using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IGameSetting : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get minimum player register by team.
        /// </summary>
        uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        /// Get maximum player register by team.
        /// </summary>
        uint MaximumPlayerByTeam { get; set; }
    }
}