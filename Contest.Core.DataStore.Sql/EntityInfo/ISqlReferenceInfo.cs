using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public interface ISqlReferenceInfo : ISqlPropertyInfo
    {
        /// <summary>
        /// Fill reference on specified item by specified unitOfWorks
        /// </summary>
        /// <param name="unitOfWorks">Unit of works used to search references</param>
        /// <param name="item">Object to fill</param>
        void FillReference(IUnitOfWorks unitOfWorks, object item);
    }
}