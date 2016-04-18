using System.Linq.Expressions;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public interface ISqlQueryFactory<in TI> where TI : class
    {
        ISqlQuery CreateTable();
        ISqlQuery Select(LambdaExpression predicate);
        ISqlQuery Insert(TI item);
        ISqlQuery Update(TI item);
        ISqlQuery Delete(TI item);
    }
}