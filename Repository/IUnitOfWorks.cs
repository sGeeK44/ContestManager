namespace Contest.Core.Repository
{
    /// <summary>
    /// Expose service to persist several object type with transaction mode.
    /// </summary>
    public interface IUnitOfWorks
    {
        /// <summary>
        /// Insert new item from repository
        /// </summary>
        /// <param name="item">New item to insert</param>
        void Insert<T>(T item) where T : class;

        /// <summary>
        /// Try to update before make an Insert of item from repository
        /// </summary>
        /// <param name="item">New or Existing item to insert/Update</param>
        void InsertOrUpdate<T>(T item) where T : class;

        /// <summary>
        /// Update existing item from repository
        /// </summary>
        /// <param name="item">Existing item to update</param>
        void Update<T>(T item) where T : class;

        /// <summary>
        /// Remove existing item from repository
        /// </summary>
        /// <param name="item">Old item to delete</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Persist all changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Undo all changes making after last commit
        /// </summary>
        void RollBack();

        /// <summary>
        /// Inject sql request for next commit
        /// </summary>
        /// <param name="request">Request to add</param>
        void AddRequest(string request);
    }
}
