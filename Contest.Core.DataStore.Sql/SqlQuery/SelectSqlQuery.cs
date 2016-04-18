using System.Linq.Expressions;
using System.Text;

namespace Contest.Core.DataStore.Sql.SqlQuery
{
    public class SelectSqlQuery<T> : SqlQuery<T>
        where T : class
    {
        private readonly SqlWhereClause<T> _sqlWhereClause;

        public SelectSqlQuery(ISqlProviderStrategy sqlProviderStrategy, LambdaExpression predicate) : base(sqlProviderStrategy)
        {
            _sqlWhereClause = new SqlWhereClause<T>(sqlProviderStrategy, predicate);
        }

        /// <summary>
        /// Translate current SqlCommand to sql string statement equivalent
        /// </summary>
        /// <returns></returns>
        public override string ToStatement()
        {
            StringBuilder columns = null;

            var columnField = SqlFieldInfo.GetSqlField<T>();
            foreach (var value in columnField)
            {
                //Add marker to set clause
                if (columns == null) columns = new StringBuilder(value.ColumnName);
                else columns.Append(", " + value.ColumnName);
            }
            
            var where = _sqlWhereClause.ToStatement();
            return string.Format(@"SELECT {0} FROM {1}{2};", columns, TableName, string.IsNullOrEmpty(where) ? string.Empty : " " + where);
        }
    }
}
