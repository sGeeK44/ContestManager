namespace Contest.Core.Repository.Sql
{
    public interface ISqlRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Persist all changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Undo all changes making after last commit
        /// </summary>
        void RollBack();

        /// <summary>
        /// Get or Set associated unit of works
        /// </summary>
        ISqlUnitOfWorks UnitOfWorks { get; set; }
    }
}
