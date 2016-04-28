using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlOneToManyReferenceInfo : SqlReferenceInfo
    {
        private readonly Type _manyToOneType;
        private readonly IList<ISqlPropertyInfo> _oneToManyPrimaryKeys;
        private readonly IList<ISqlPropertyInfo> _manyToOneForeignKeys;

        /// <summary>
        /// Get SqlField use as key in predicate
        /// </summary>
        protected override IList<ISqlPropertyInfo> Key { get { return _oneToManyPrimaryKeys; } }

        /// <summary>
        /// Get SqlField to use as reference for predicate. 
        /// </summary>
        protected override IList<ISqlPropertyInfo> ReferenceKey { get { return _manyToOneForeignKeys; } }

        /// <summary>
        /// Get type of reference to use in predicate parameter
        /// </summary>
        protected override Type ReferenceType { get { return _manyToOneType; } }

        internal SqlOneToManyReferenceInfo(PropertyInfo referenceProperty, Type manyToOneType, IList<ISqlPropertyInfo> oneToManyPrimaryKeys, IList<ISqlPropertyInfo> manyToOneForeignKeys)
            : base(referenceProperty)
        {
            _manyToOneType = manyToOneType;
            _oneToManyPrimaryKeys = oneToManyPrimaryKeys;
            _manyToOneForeignKeys = manyToOneForeignKeys;
        }

        protected SqlOneToManyReferenceInfo(SqlOneToManyReferenceInfo item)
            : base(item.PropertyInfo)
        {
            _manyToOneType = item._manyToOneType;
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
            return unitOfWorks != null
                 ? unitOfWorks.Find(ReferenceType, GetPredicate(item))
                 : CreateEmptyListReference();
        }

        private IList CreateEmptyListReference()
        {
            var typeList = typeof(List<>).MakeGenericType(ReferenceType);
            return Activator.CreateInstance(typeList) as IList;
        }
    }
}
