using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.BusinessObjectFactory
{
    public interface IBusinessObjectFactory<T>
    {
        /// <summary>
        /// Convert specified object to an business object T
        /// </summary>
        /// <param name="from">Object representation origin</param>
        /// <returns>Business object converted</returns>
        T Convert(object from);

        /// <summary>
        /// Fill Rerence property with object serach with specified unit of works
        /// </summary>
        /// <param name="unitOfWorks">Unit of works used to find reference</param>
        /// <param name="item">Item to fill</param>
        void FillReferences(IUnitOfWorks unitOfWorks, T item);
    }
}
