using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public abstract class SqlReferenceInfo : SqlPropertyInfo, ISqlReferenceInfo
    {
        /// <summary>
        /// Get SqlField use as key in predicate
        /// </summary>
        protected abstract IList<ISqlFieldInfo> Key { get; }

        /// <summary>
        /// Get SqlField to use as reference for predicate. 
        /// </summary>
        protected abstract IList<ISqlFieldInfo> ReferenceKey { get; }

        /// <summary>
        /// Get type of reference to use in predicate parameter
        /// </summary>
        protected abstract Type ReferenceType { get; }

        /// <summary>
        /// Initialize a new Instance for spcified reference property
        /// </summary>
        /// <param name="propertyInfo">Property to link</param>
        protected SqlReferenceInfo(PropertyInfo propertyInfo)
            : base(propertyInfo) { }

        /// <summary>
        /// Build a Lambda expression to retreive references
        /// </summary>
        /// <param name="item">Object on wich reference have to fill</param>
        /// <returns>A Lambda expression like that : (ReferenceType)_ => _.ReferenceKey == item.Key</returns>
        protected LambdaExpression GetPredicate(object item)
        {
            var referenceEntity = Expression.Parameter(ReferenceType, "_");
            var referenceKeyProp = Expression.Property(referenceEntity, ReferenceKey[0].PropertyInfo);
            var innerKeyProp = Expression.MakeMemberAccess(Expression.Constant(item), Key[0].PropertyInfo);

            var condition = Expression.Equal(referenceKeyProp, innerKeyProp);
            var type = typeof(Func<,>).MakeGenericType(ReferenceType, typeof(bool));
            return Expression.Lambda(type, condition, new[] { referenceEntity });
        }

        /// <summary>
        /// Search reference value in specified unit of works for specified item
        /// </summary>
        /// <param name="unitOfWorks">UnitOfWorks wich contains repository of references</param>
        /// <param name="item">Object on wich reference have to fill</param>
        /// <returns>Reference value</returns>
        protected abstract object FindReferenceValue(IUnitOfWorks unitOfWorks, object item);

        /// <summary>
        /// Fill reference property of specified item
        /// </summary>
        /// <param name="unitOfWorks">UnitOfWorks wich contains repository of references</param>
        /// <param name="item">Object on wich reference have to fill</param>
        public void FillReference(IUnitOfWorks unitOfWorks, object item)
        {
            if (item == null) return;

            SetValue(item, FindReferenceValue(unitOfWorks, item));
        }
    }
}
