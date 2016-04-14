namespace Contest.Core.DataStore.Sql.BusinessObjectFactory
{
    public interface IBusinessObjectFactory<out T>
    {
        /// <summary>
        /// Convert specified object to an business object T
        /// </summary>
        /// <param name="from">Object representation origin</param>
        /// <returns>Business object converted</returns>
        T Convert(object from);
    }
}
