using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contest.Domain
{
    /// <summary>
    ///     Expose command feature expose by a standard repository
    /// </summary>
    /// <typeparam name="TIEntity">Type of entity managed by repository</typeparam>
    public interface IRepository<TEntity, TIEntity>
    {
        List<TIEntity> GetAll();

        TIEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TIEntity> Find(Expression<Func<TEntity, bool>> filter);
    }
}