using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contest.Core.DataStore.Sql.EntityInfo;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlManyToManyReferenceInfoTester : SqlManyToManyReferenceInfo
    {
        public IList<ISqlFieldInfo> KeyTester { get { return Key; } }
        public IList<ISqlFieldInfo> ReferenceKeyTester { get { return ReferenceKey; } }
        public Type ReferenceTypeTester { get { return ReferenceType; } }

        protected SqlManyToManyReferenceInfoTester(SqlManyToManyReferenceInfo @base)
            : base(@base) { }
        
        public static List<SqlManyToManyReferenceInfoTester> GetSqlReferenceTester<T>()
        {
            return EntityInfoFactory.GetManyToManySqlReference<T>().Select(_ => new SqlManyToManyReferenceInfoTester(_ as SqlManyToManyReferenceInfo)).ToList();
        }

        public LambdaExpression GetLambdaExpression(object item)
        {
            return GetPredicate(item);
        }
    }
}