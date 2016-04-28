using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.EntityInfo;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlOneToManyReferenceInfoTester : SqlOneToManyReferenceInfo
    {
        public IList<ISqlPropertyInfo> KeyTester { get { return Key; } }
        public IList<ISqlPropertyInfo> ReferenceKeyTester { get { return ReferenceKey; } }
        public Type ReferenceTypeTester { get { return ReferenceType; } }

        protected SqlOneToManyReferenceInfoTester(SqlOneToManyReferenceInfo @base)
            : base(@base) { }

        public static List<SqlOneToManyReferenceInfoTester> GetSqlReferenceTester<T>()
        {
            return EntityInfoFactory.GetOneToManySqlReference<T>().Select(_ => new SqlOneToManyReferenceInfoTester(_)).ToList();
        }
    }
}