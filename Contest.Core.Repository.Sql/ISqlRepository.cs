namespace Contest.Core.Repository.Sql
{
    public interface ISqlRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Get or Set associated unit of works
        /// </summary>
        ISqlUnitOfWorks UnitOfWorks { get; set; }
    }
}
