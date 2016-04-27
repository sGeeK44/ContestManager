using System;
using System.Collections;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql
{
    public interface ISqlRepository<T> : ISqlRepository, IRepository<T> where T : class { }
    public interface ISqlRepository
    {
        /// <summary>
        /// Prepare request for create table on managed object
        /// </summary>
        void CreateTable();

        /// <summary>
        /// Search items in datastore
        /// </summary>
        /// <param name="objectTypeSearch">Type of inner list element to search</param>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList Find(Type objectTypeSearch, LambdaExpression predicate);

        /// <summary>
        /// Search items in loaed context
        /// </summary>
        /// <param name="objectTypeSearch">Type of inner list element to search</param>
        /// <param name="predicate">NewA lamba expression for where clause</param>
        /// <return>A list wich contain all item founds or an empty list</return>
        IList FindInContext(Type objectTypeSearch, LambdaExpression predicate);

        /// <summary>
        /// Get or Set associated unit of works
        /// </summary>
        ISqlUnitOfWorks UnitOfWorks { get; set; }
    }
}
