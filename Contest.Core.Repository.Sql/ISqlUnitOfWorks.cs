using Contest.Core.DataStore.Sql;

namespace Contest.Core.Repository.Sql
{
    public interface ISqlUnitOfWorks : IUnitOfWorks
    {
        /// <summary>
        /// Indicate if current unit of work is mapped to a Database
        /// </summary>
        bool IsBinded { get; }

        /// <summary>
        /// Get store used for data access
        /// </summary>
        ISqlDataStore SqlDataStore { get; }

        /// <summary>
        /// Get boolean to know if specified repository is already present in current unit of work
        /// </summary>
        /// <typeparam name="T">Type of object managed by repository</typeparam>
        /// <param name="repository">Repository to test</param>
        /// <returns>True if repository is already present, false else</returns>
        bool ConstainsRepository<T>(ISqlRepository<T> repository) where T : class;

        /// <summary>
        /// Create link between specified repository and current unit of works
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        void AddRepository<T>(ISqlRepository<T> repository) where T : class;
    }
}
