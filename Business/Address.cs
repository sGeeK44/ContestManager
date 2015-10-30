using System;
using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent an address.
    /// </summary>
    [DataContract(Name = "ADDRESS")]
    public class Address : Identifiable<Address>, IAddress
    {
        #region Constructors

        private Address() { }

        #endregion

        #region Properties

        /// <summary>
        /// Get number in street of address
        /// </summary>
        [DataMember(Name = "STREET_NUMBER")]
        public int StreetNumber { get; set; }

        /// <summary>
        /// Get street of address
        /// </summary>
        [DataMember(Name = "STREET")]
        public string Street { get; set; }

        /// <summary>
        /// Get zip code of address
        /// </summary>
        [DataMember(Name = "ZIP_CODE")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Get city of address
        /// </summary>
        [DataMember(Name = "CITY")]
        public string City { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IAddress>(this);
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
        /// Create a new instance (in memory and database) of <see cref="T:Business.Address"/> with specified param
        /// </summary>
        /// <returns>Match's instance</returns>
        public static IAddress Create(int streetNumber, string street, string zipCode, string city)
        {
            var result = new Address
            {
                Id = Guid.NewGuid(),
                StreetNumber = streetNumber,
                Street = street,
                ZipCode = zipCode,
                City = city
            };

            return result;
        }

        #endregion
    }
}
