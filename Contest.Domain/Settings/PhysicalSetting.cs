using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Domain.Players;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

namespace Contest.Domain.Settings
{
    /// <summary>
    ///     Represent all physical setting for a tournament.
    /// </summary>
    [Entity(NameInStore = "PHYSICAL_SETTING")]
    public class PhysicalSetting : Entity, IPhysicalSetting
    {
        #region Fields

        private ReferenceHolder<IAddress, Guid> _address;

        #endregion

        #region Constructors

        public PhysicalSetting()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _address = new ReferenceHolder<IAddress, Guid>(AddressRepository);
        }

        #endregion

        #region MEF Import

        [Import] private IRepository<Address, IAddress> AddressRepository { get; set; }

        #endregion

        #region Factory

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.PhysicalSetting" /> with specified param
        /// </summary>
        /// <param name="address">Address of tournament</param>
        /// <param name="type">Physical type of tournament.</param>
        /// <param name="countField">Number of field available for contest</param>
        /// <returns>PhysicalSetting's instance</returns>
        public static IPhysicalSetting Create(IAddress address, AreaType type, ushort countField)
        {
            if (countField == 0) throw new ArgumentException("Il faut au moins un terrain de disponible.");
            var result = new PhysicalSetting
            {
                Address = address,
                PhysicalType = type,
                CountField = countField
            };

            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get address id of physical setting
        /// </summary>
        public Guid AddressId
        {
            get => _address.Id;
            private set => _address.Id = value;
        }


        /// <summary>
        ///     Get address of tournament
        /// </summary>
        public IAddress Address
        {
            get => _address.Object;
            set => _address.Object = value;
        }

        /// <summary>
        ///     Get physical type
        /// </summary>
        [Field(FieldName = "TYPE")]
        public AreaType PhysicalType { get; set; }

        /// <summary>
        ///     Get number of field available for contest
        /// </summary>
        [Field(FieldName = "NUMBER_FIELD")]
        public ushort CountField { get; set; }

        #endregion
    }
}