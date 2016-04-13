using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql
{
    public interface ISqlBuilder<T, TI> where T : class, TI where TI : class
    {
        TI CreateInstance(IDataReader row);
        string CreateTable();
        string Select(Expression<Func<TI, bool>> predicate, out IList<SqlField> arg);
        string Insert(TI item, out IList<SqlField> arg);
        string Update(TI item, out IList<SqlField> arg);
        string Delete(TI item, out IList<SqlField> arg);
    }
}