using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public abstract class SqlReferenceInfo : SqlPropertyInfo
    {
        /// <summary>
        /// Get SqlField use as key in predicate
        /// </summary>
        protected abstract IList<ISqlPropertyInfo> Key { get; }

        /// <summary>
        /// Get SqlField to use as reference for predicate. 
        /// </summary>
        protected abstract IList<ISqlPropertyInfo> ReferenceKey { get; }

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

        /// <summary>
        /// Get sql column name
        /// </summary>
        public override string ColumnName { get { throw new NotSupportedException("Reference property can not provide sql column name."); } }

        /// <summary>
        /// Indicate if current property is a primary key
        /// </summary>
        public override bool IsPrimaryKey { get { throw new NotSupportedException("Reference property can not be a primary key."); } }

        /// <summary>
        /// Indicate if current property is a foreign key of specified property
        /// </summary>
        /// <param name="prop">ManyToOne property</param>
        /// <returns>True if current part is foreign key, false else</returns>
        public override bool IsForeignKeyOf(PropertyInfo prop)
        {
            throw new NotSupportedException("Reference property can not provide sql value equivalent.");
        }

        /// <summary>
        /// Convert property value on specified entity to sql string value equivalent
        /// </summary>
        /// <param name="sqlProviderStrategy">Strategy used to converter value</param>
        /// <param name="entity">Entity on wich getting property value</param>
        /// <returns>Sql string value equivalent</returns>
        public override string ToSqlValue(ISqlProviderStrategy sqlProviderStrategy, object entity)
        {
            throw new NotSupportedException("Reference property can not be a foreign key.");
        }
    }
}
