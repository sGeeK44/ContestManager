using System.Data;

namespace Contest.Core.Repository.Sql
{
    public interface IBusinessObjectFactory<out T>
    {
        /// <summary>
        /// Convert sql row result to an business object T
        /// </summary>
        /// <param name="resultRow">Sql row result</param>
        /// <returns>Business object</returns>
        T Create(IDataReader resultRow);
    }
}
