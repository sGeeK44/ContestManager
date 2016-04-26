using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql
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

        private SqlManyToManyReferenceInfo(PropertyInfo propertyInfo, IList<ISqlPropertyInfo> entityPrimaryKeys, Type relationType, IList<ISqlPropertyInfo> relationProperty)
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

        /// <summary>
        /// Get ManyToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToManyReferenceInfo of specified type</returns>
        public static List<SqlManyToManyReferenceInfo> GetSqlReference<T>()
        {
            var sqlProperties = GetPropertiesList<T>();
            var entityPrimaryKeys = SqlFieldInfo.GetPrimaryKeys(sqlProperties).Cast<ISqlPropertyInfo>().ToList();

            return sqlProperties.Where(_ => _.IsDefined(typeof(SqlManyToManyReferenceAttribute)))
                                             .Select(_ => Create<T>(_, entityPrimaryKeys))
                                             .ToList();
        }

        private static SqlManyToManyReferenceInfo Create<T>(PropertyInfo entityProperty, IList<ISqlPropertyInfo> entityPrimaryKeys)
        {
            var propertyType = entityProperty.PropertyType;
            if (!IsList(propertyType)) throw new NotSupportedException(string.Format("Property type for ManyToMany reference have to be an IList<>. Property:{0}.", entityProperty.Name));

            var referenceType = propertyType.GenericTypeArguments[0];
            if (!IsRelationShip(referenceType)) throw new NotSupportedException(string.Format("Reference for ManyToMany relation have to be an Relation<,>. Property:{0}.", entityProperty.Name));

            var firstRelationType = referenceType.GenericTypeArguments[0];
            
            var referenceSqlProperties = GetPropertiesList(referenceType).Where(_ => _.Name == (IsAnalyseEntity<T>(firstRelationType) ? "FirstItemInvolveId" : "SecondItemInvolveId"));
            var referenceForeignKeys = SqlFieldInfo.GetPrimaryKeys(referenceSqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return new SqlManyToManyReferenceInfo(entityProperty, entityPrimaryKeys, referenceType, referenceForeignKeys);
        }

        private static bool IsAnalyseEntity<T>(Type type)
        {
            return type == typeof(T);
        }

        private IList CreateEmptyListReference()
        {
            var typeList = typeof(List<>).MakeGenericType(ReferenceType);
            return Activator.CreateInstance(typeList) as IList;
        }

        private static bool IsList(Type t)
        {
            if (t == null) throw new NullReferenceException();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>);
        }

        private static bool IsRelationShip(Type t)
        {
            if (t == null) throw new NullReferenceException();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Relationship<,>);
        }
    }
}
