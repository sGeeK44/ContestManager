namespace Contest.Domain.Settings
{
    /// <summary>
    ///     Expose method's to create instance of IAddress
    /// </summary>
    public interface IAddressFactory
    {
        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Address" /> with specified param
        /// </summary>
        /// <returns>Match's instance</returns>
        IAddress Create(int streetNumber, string street, string zipCode, string city);
    }
}