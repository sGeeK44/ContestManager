using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    public class SqlReferenceInfoTester : SqlReferenceInfo
    {
        private readonly IList<ISqlPropertyInfo> _key;
        private readonly IList<ISqlPropertyInfo> _referenceKey;
        private readonly Type _referenceType;

        public SqlReferenceInfoTester(PropertyInfo referenceProperty, Type type, IList<ISqlPropertyInfo> referenceKeyList, IList<ISqlPropertyInfo> keyList)
            : base(referenceProperty)
        {
            _referenceType = type;
            _referenceKey = referenceKeyList;
            _key = keyList;
        }

        public LambdaExpression GetLambdaExpression(object item)
        {
            return GetPredicate(item);
        }

        protected override IList<ISqlPropertyInfo> Key
        {
            get { return _key; }
        }

        protected override IList<ISqlPropertyInfo> ReferenceKey
        {
            get { return _referenceKey; }
        }

        protected override Type ReferenceType
        {
            get { return _referenceType; }
        }

        protected override object FindReferenceValue(IUnitOfWorks unitOfWorks, object item)
        {
            throw new NotImplementedException();
        }
    }
}