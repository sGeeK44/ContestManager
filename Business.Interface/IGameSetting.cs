using System.Runtime.Serialization;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IGameSetting : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get minimum player register by team.
        /// </summary>
        [DataMember(Name = "MIN_PLAYER_PER_TEAM")]
        uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        /// Get maximum player register by team.
        /// </summary>
        [DataMember(Name = "MAX_PLAYER_PER_TEAM")]
        uint MaximumPlayerByTeam { get; set; }
    }
}