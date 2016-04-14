using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql;

namespace Contest.Core.Repository.Sql
{
    public interface ISqlUnitOfWorks : IUnitOfWorks
    {
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

        /// <summary>
        /// Search item in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <retur>First item found or default(T)</retur>
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Search items in repository
        /// </summary>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Indicate if current unit of work is mapped to a Database
        /// </summary>
        bool IsBinded { get; }

        ISqlDataStore SqlDataStore { get; }
    }
}
