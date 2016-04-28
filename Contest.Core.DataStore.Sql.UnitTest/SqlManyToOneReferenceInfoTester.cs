using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.EntityInfo;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlManyToOneReferenceInfoTester : SqlManyToOneReferenceInfo
    {
        public IList<ISqlFieldInfo> KeyTester { get { return Key; } }
        public IList<ISqlFieldInfo> ReferenceKeyTester { get { return ReferenceKey; } }
        public Type ReferenceTypeTester { get { return ReferenceType; } }

        protected SqlManyToOneReferenceInfoTester(SqlManyToOneReferenceInfo @base)
            : base(@base) { }

        public static List<SqlManyToOneReferenceInfoTester> GetSqlReferenceTester<T>()
        {
            return EntityInfoFactory.GetManyToOneSqlReference<T>().Select(_ => new SqlManyToOneReferenceInfoTester(_ as SqlManyToOneReferenceInfo)).ToList();
        }
    }
}