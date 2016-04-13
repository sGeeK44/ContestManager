using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public class SqlBuilderMock<T, TI> : ISqlBuilder<T, TI> where T : class, TI where TI : class
    {
        public TI CreateInstance(IDataReader row)
        {
            return default(T);
        }

        public string CreateTable()
        {
            return string.Empty;
        }

        public string Select(Expression<Func<TI, bool>> predicate, out IList<SqlField> arg)
        {
            arg = new SqlField[0];
            return string.Empty;
        }

        public string Insert(TI item, out IList<SqlField> arg)
        {
            CountInsertCall++;
            arg = new SqlField[0];
            return string.Empty;
        }

        public int CountInsertCall { get; set; }
        public int CountUpdateCall { get; set; }
        public int CountDeleteCall { get; set; }

        public string Update(TI item, out IList<SqlField> arg)
        {
            CountUpdateCall++;
            arg = new SqlField[0];
            return string.Empty;
        }

        public string Delete(TI item, out IList<SqlField> arg)
        {
            CountDeleteCall++;
            arg = new SqlField[0];
            return string.Empty;
        }

        public string ToSqlType(Type objectType)
        {
            return string.Empty;
        }
    }
}
