using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SmartWay.Orm.Repositories;

namespace Contest.Domain
{
    /// <summary>
    ///     Expose command feature expose by a standard repository
    /// </summary>
    /// <typeparam name="TEntity">Type of concrete entity managed by repository</typeparam>
    /// <typeparam name="TIEntity">Type of entity managed by repository</typeparam>
    public interface IRepository<TEntity, TIEntity>: IRepository<TIEntity>
    {
        TIEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TIEntity> Find(Expression<Func<TEntity, bool>> filter);
    }
}