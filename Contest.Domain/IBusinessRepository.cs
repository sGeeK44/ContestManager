using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contest.Domain
{
    /// <summary>
    ///     Expose command feature expose by a standard repository
    /// </summary>
    /// <typeparam name="TEntity">Type of entity managed by repository</typeparam>
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter);
    }
}