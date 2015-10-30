namespace Contest.Core.Repository.Sql
{
    public interface ISqlPersistable
    {
        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        void PrepareCommit(ISqlUnitOfWorks unitOfWorks);

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        void PrepareDelete(ISqlUnitOfWorks unitOfWorks);
    }
}
