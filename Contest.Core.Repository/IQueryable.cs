namespace Contest.Core.Repository
{
    public interface IQueryable
    {
        /// <summary>
        /// Determine if specified object is same object than current
        /// </summary>
        /// <returns>True if they are identic, false else</returns>
        bool AreSame(object other);
    }
}
