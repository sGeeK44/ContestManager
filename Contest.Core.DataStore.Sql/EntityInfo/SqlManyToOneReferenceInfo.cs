using System;
using System.Collections.Generic;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlManyToOneReferenceInfo : SqlReferenceInfo
    {
        private readonly Type _oneToManyType;
        private readonly IList<ISqlPropertyInfo> _oneToManyPrimaryKeys;
        private readonly IList<ISqlPropertyInfo> _manyToOneForeignKeys;

        /// <summary>
        /// Get SqlField use as key in predicate
        /// </summary>
        protected override IList<ISqlPropertyInfo> Key { get { return _manyToOneForeignKeys; } }

        /// <summary>
        /// Get SqlField to use as reference for predicate. 
        /// </summary>
        protected override IList<ISqlPropertyInfo> ReferenceKey { get { return _oneToManyPrimaryKeys; } }

        /// <summary>
        /// Get type of reference to use in predicate parameter
        /// </summary>
        protected override Type ReferenceType {  get { return _oneToManyType; } }

        internal SqlManyToOneReferenceInfo(PropertyInfo referenceProperty, Type oneToManyType, IList<ISqlPropertyInfo> oneToManyPrimaryKeys, IList<ISqlPropertyInfo> manyToOneForeignKeys)
            : base(referenceProperty)
        {
            if (oneToManyType == null) throw new ArgumentNullException("oneToManyType");
            if (oneToManyPrimaryKeys == null) throw new ArgumentNullException("oneToManyPrimaryKeys");
            if (manyToOneForeignKeys == null) throw new ArgumentNullException("manyToOneForeignKeys");
            if (oneToManyPrimaryKeys.Count == 0) throw new ArgumentException("Need one primary key property at less");
            if (manyToOneForeignKeys.Count == 0) throw new ArgumentException("Need one foreign key property at less");

            _oneToManyType = oneToManyType;
            _oneToManyPrimaryKeys = oneToManyPrimaryKeys;
            _manyToOneForeignKeys = manyToOneForeignKeys;
        }

        protected SqlManyToOneReferenceInfo(SqlManyToOneReferenceInfo item)
            : base(item.PropertyInfo)
        {
            _oneToManyType = item._oneToManyType;
            _oneToManyPrimaryKeys = item._oneToManyPrimaryKeys;
            _manyToOneForeignKeys = item._manyToOneForeignKeys;
        }

        /// <summary>
        /// Search reference value in specified unit of works for specified item
        /// </summary>
        /// <param name="unitOfWorks">UnitOfWorks wich contains repository of references</param>
        /// <param name="item">Object on wich reference have to fill</param>
        /// <returns>Reference value</returns>
        protected override object FindReferenceValue(IUnitOfWorks unitOfWorks, object item)
        {
            if (unitOfWorks == null) return null;

            var list = unitOfWorks.FindInContext(ReferenceType, GetPredicate(item));
            if (list != null && list.Count == 1) return list[0];

            list = unitOfWorks.Find(ReferenceType, GetPredicate(item));
            if (list == null || list.Count != 1) return null;

            return list[0];
        }
    }
}
