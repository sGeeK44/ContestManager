using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent all physical setting for a tournament.
    /// </summary>
    [SqlEntity(Name = "PHYSICAL_SETTING")]
    public class PhysicalSetting : Identifiable<PhysicalSetting>, IPhysicalSetting
    {
        #region Fields

        private Lazy<IAddress> _address;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IAddress> AddressRepository { get; set; }

        #endregion

        #region Constructors

        private PhysicalSetting()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _address = new Lazy<IAddress>(() => AddressRepository.FirstOrDefault(_ => _.Id == AddressId));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get address id of physical setting
        /// </summary>
        public Guid AddressId { get; private set; }


        /// <summary>
        /// Get address of tournament
        /// </summary>
        public IAddress Address
        {
            get { return _address.Value; }
            set
            {
                _address = new Lazy<IAddress>(() => value);
                AddressId = value != null ? value.Id : Guid.Empty;
            }
        }

        /// <summary>
        /// Get physical type
        /// </summary>
        [SqlField(Name = "TYPE")]
        public AreaType PhysicalType { get; set; }

        /// <summary>
        /// Get number of field available for contest
        /// </summary>
        [SqlField(Name = "NUMBER_FIELD")]
        public ushort CountField { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IPhysicalSetting>(this);
            if (Address != null) Address.PrepareCommit(unitOfWorks);
        }

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.PhysicalSetting"/> with specified param
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
                    Id = Guid.NewGuid(),
                    Address = address,
                    PhysicalType = type,
                    CountField = countField
                };

            return result;
        }

        #endregion
    }
}
