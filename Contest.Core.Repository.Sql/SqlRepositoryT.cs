namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Expose methods for CRUD action on T base on Sql database for persistance
    /// </summary>
    /// <typeparam name="T">Type of repository object</typeparam>
    public class SqlRepository<T> : SqlRepository<T, T> where T : class, IQueryable { }
}
