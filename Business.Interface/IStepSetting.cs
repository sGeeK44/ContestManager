using System;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IStepSetting : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get match's setting for  step
        /// </summary>
        Guid MatchSettingId { get; }

        /// <summary>
        /// Get match's setting for  step
        /// </summary>
        IMatchSetting MatchSetting { get; }
    }
}