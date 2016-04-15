using System;
using System.Linq.Expressions;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public interface ISqlQueryFactory<TI> where TI : class
    {
        ISqlQuery CreateTable();
        ISqlQuery Select(Expression<Func<TI, bool>> predicate);
        ISqlQuery Insert(TI item);
        ISqlQuery Update(TI item);
        ISqlQuery Delete(TI item);
    }
}