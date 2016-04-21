using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql
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

        private SqlOneToManyReferenceInfo(PropertyInfo referenceProperty, Type manyToOneType, IList<ISqlPropertyInfo> oneToManyPrimaryKeys, IList<ISqlPropertyInfo> manyToOneForeignKeys)
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

        /// <summary>
        /// Get OneToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlOneToManyReferenceInfo of specified type</returns>
        public static List<SqlOneToManyReferenceInfo> GetSqlReference<T>()
        {
            var oneToManySqlProperties = GetPropertiesList<T>();
            var oneToManyPrimaryKeys = SqlFieldInfo.GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlPropertyInfo>().ToList();

            return oneToManySqlProperties.Where(_ => _.IsDefined(typeof(SqlOneToManyReferenceAttribute)))
                                         .Select(_ => Create(_, oneToManyPrimaryKeys))
                                         .ToList();
        }

        private static SqlOneToManyReferenceInfo Create(PropertyInfo oneToManyProperty, IList<ISqlPropertyInfo> oneToManyPrimaryKeys)
        {
            if (!IsList(oneToManyProperty.PropertyType)) throw new NotSupportedException(string.Format("Property type for OneToMany reference have to be an IList<>. Property:{0}.", oneToManyProperty.Name));

            var manyToOneType = oneToManyProperty.PropertyType.GetGenericArguments()[0];
            var manyToOneSqlProperties = GetPropertiesList(manyToOneType);
            var manyToOneForeignKeys = SqlFieldInfo.GetForeignKeys(manyToOneSqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return new SqlOneToManyReferenceInfo(oneToManyProperty, manyToOneType, oneToManyPrimaryKeys, manyToOneForeignKeys);
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
    }
}
