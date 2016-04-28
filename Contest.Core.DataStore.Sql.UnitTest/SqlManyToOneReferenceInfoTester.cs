using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.EntityInfo;

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
            return EntityInfoFactory.GetManyToOneSqlReference<T>().Select(_ => new SqlManyToOneReferenceInfoTester(_)).ToList();
        }
    }
}