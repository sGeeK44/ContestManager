using SmartWay.Orm.Attributes;

namespace Contest.Domain.Settings
{
    /// <summary>
    ///     Represent an address.
    /// </summary>
    [Entity(NameInStore = "ADDRESS")]
    public class Address : Entity, IAddress
    {
        #region Properties

        /// <summary>
        ///     Get number in street of address
        /// </summary>
        [Field(FieldName = "STREET_NUMBER")]
        public int StreetNumber { get; set; }

        /// <summary>
        ///     Get street of address
        /// </summary>
        [Field(FieldName = "STREET")]
        public string Street { get; set; }

        /// <summary>
        ///     Get zip code of address
        /// </summary>
        [Field(FieldName = "ZIP_CODE")]
        public string ZipCode { get; set; }

        /// <summary>
        ///     Get city of address
        /// </summary>
        [Field(FieldName = "CITY")]
        public string City { get; set; }

        #endregion
    }
}