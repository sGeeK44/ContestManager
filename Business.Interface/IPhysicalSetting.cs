using System;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IPhysicalSetting : IIdentifiable, IQueryable, ISqlPersistable
    {
        /// <summary>
        /// Get address id of physical setting
        /// </summary>
        Guid AddressId { get; }

        /// <summary>
        /// Get address of tournament
        /// </summary>
        IAddress Address { get; set; }

        /// <summary>
        /// Get physical type
        /// </summary>
        AreaType PhysicalType { get; set; }

        /// <summary>
        /// Get number of field available for contest
        /// </summary>
        ushort CountField { get; set; }
    }
}