namespace Contest.Core.DataStore.Sql.SqlQuery
{
    /// <summary>
    /// Expose methods to convert SqlCommand object to string sql command
    /// </summary>
    public interface ISqlQuery
    {
        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        string ToStatement();
    }
}
