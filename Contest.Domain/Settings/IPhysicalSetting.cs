using System;

namespace Contest.Domain.Settings
{
    public interface IPhysicalSetting : IEntity
    {
        /// <summary>
        ///     Get address id of physical setting
        /// </summary>
        Guid AddressId { get; }

        /// <summary>
        ///     Get address of tournament
        /// </summary>
        IAddress Address { get; set; }

        /// <summary>
        ///     Get physical type
        /// </summary>
        AreaType PhysicalType { get; set; }

        /// <summary>
        ///     Get number of field available for contest
        /// </summary>
        ushort CountField { get; set; }
    }
}