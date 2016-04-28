using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class SqlManyToManyReferenceInfo : SqlReferenceInfo
    {
        private readonly Type _relationType;
        private readonly IList<ISqlPropertyInfo> _entityPrimaryKeys;
        private readonly IList<ISqlPropertyInfo> _relationProperty;

        /// <summary>
        /// Get SqlField use as key in predicate
        /// </summary>
        protected override IList<ISqlPropertyInfo> Key { get { return _entityPrimaryKeys; } }

        /// <summary>
        /// Get SqlField to use as reference for predicate. 
        /// </summary>
        protected override IList<ISqlPropertyInfo> ReferenceKey { get { return _relationProperty; } }

        /// <summary>
        /// Get type of reference to use in predicate parameter
        /// </summary>
        protected override Type ReferenceType { get { return _relationType; } }

        internal SqlManyToManyReferenceInfo(PropertyInfo propertyInfo, IList<ISqlPropertyInfo> entityPrimaryKeys, Type relationType, IList<ISqlPropertyInfo> relationProperty)
            : base(propertyInfo)
        {
            _relationType = relationType;
            _entityPrimaryKeys = entityPrimaryKeys;
            _relationProperty = relationProperty;
        }

        protected SqlManyToManyReferenceInfo(SqlManyToManyReferenceInfo item)
            : base(item.PropertyInfo)
        {
            _relationType = item._relationType;
            _entityPrimaryKeys = item._entityPrimaryKeys;
            _relationProperty = item._relationProperty;
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
