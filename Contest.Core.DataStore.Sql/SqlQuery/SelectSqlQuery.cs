using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class SelectSqlQuery<T, TI> : SqlQuery<T>
        where T : class, TI
        where TI : class
    {
        private readonly SqlWhereClause<T, TI> _sqlWhereClause;

        public SelectSqlQuery(ISqlProviderStrategy sqlProviderStrategy, Expression<Func<TI, bool>> predicate) : base(sqlProviderStrategy)
        {
            _sqlWhereClause = new SqlWhereClause<T, TI>(predicate);
        }

        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        public override string ToStatement()
        {
            StringBuilder columns = null;

            var columnField = SqlFieldExtension.GetSqlField<T>(null);
            foreach (var value in columnField)
            {
                //Add marker to set clause
                if (columns == null) columns = new StringBuilder(value.ColumnName);
                else columns.Append(", " + value.ColumnName);
            }

            IList<SqlColumnField> arg;
            var where = _sqlWhereClause.ToStatement(out arg);
            where = arg.Aggregate(where, (current, sqlColumnField) => current.Replace(sqlColumnField.MarkerValue, sqlColumnField.ToSqlValue(SqlProviderStrategy)));
            return string.Format(@"SELECT {0} FROM {1}{2};", columns, TableName, string.IsNullOrEmpty(where) ? string.Empty : " " + where);
        }
    }
}
