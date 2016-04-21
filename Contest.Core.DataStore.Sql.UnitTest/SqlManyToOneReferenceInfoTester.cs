using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlManyToOneReferenceInfoTester : SqlManyToOneReferenceInfo
    {
        public IList<ISqlPropertyInfo> KeyTester { get { return Key; } }
        public IList<ISqlPropertyInfo> ReferenceKeyTester { get { return ReferenceKey; } }
        public Type ReferenceTypeTester { get { return ReferenceType; } }

        protected SqlManyToOneReferenceInfoTester(SqlManyToOneReferenceInfo @base)
            : base(@base) { }

        public static List<SqlManyToOneReferenceInfoTester> GetSqlReferenceTester<T>()
        {
            return GetSqlReference<T>().Select(_ => new SqlManyToOneReferenceInfoTester(_)).ToList();
        }
    }
}