using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlManyToManyReferenceInfoTester : SqlManyToManyReferenceInfo
    {
        public IList<ISqlPropertyInfo> KeyTester { get { return Key; } }
        public IList<ISqlPropertyInfo> ReferenceKeyTester { get { return ReferenceKey; } }
        public Type ReferenceTypeTester { get { return ReferenceType; } }

        protected SqlManyToManyReferenceInfoTester(SqlManyToManyReferenceInfo @base)
            : base(@base) { }
        
        public static List<SqlManyToManyReferenceInfoTester> GetSqlReferenceTester<T>()
        {
            return EntityInfoFactory.GetManyToManySqlReference<T>().Select(_ => new SqlManyToManyReferenceInfoTester(_)).ToList();
        }

        public LambdaExpression GetLambdaExpression(object item)
        {
            return GetPredicate(item);
        }
    }
}