using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    public interface IAddress : IIdentifiable, ISqlPersistable
    {
        /// <summary>
        /// Get number in street of address
        /// </summary>
        int StreetNumber { get; set; }

        /// <summary>
        /// Get street of address
        /// </summary>
        string Street { get; set; }

        /// <summary>
        /// Get zip code of address
        /// </summary>
        string ZipCode { get; set; }

        /// <summary>
        /// Get city of address
        /// </summary>
        string City { get; set; }
    }
}