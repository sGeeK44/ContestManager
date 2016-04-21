using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql
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

        private SqlManyToOneReferenceInfo(PropertyInfo referenceProperty, Type oneToManyType, IList<ISqlPropertyInfo> oneToManyPrimaryKeys, IList<ISqlPropertyInfo> manyToOneForeignKeys)
            : base(referenceProperty)
        {
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

            var list = unitOfWorks.Find(ReferenceType, GetPredicate(item));
            if (list == null || list.Count != 1) return null;

            return list[0];
        }

        /// <summary>
        /// Get ManyToOne reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToOneReferenceInfo of specified type</returns>
        public static List<SqlManyToOneReferenceInfo> GetSqlReference<T>()
        {
            var manyToOneSqlProperties = GetPropertiesList<T>();
            var manyToOneForeignKeys = SqlFieldInfo.GetForeignKeys(manyToOneSqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return manyToOneSqlProperties.Where(_ => _.IsDefined(typeof(SqlManyToOneReferenceAttribute)))
                                          .Select(_ => Create(_, manyToOneForeignKeys))
                                          .ToList();
        }

        private static SqlManyToOneReferenceInfo Create(PropertyInfo prop, IList<ISqlPropertyInfo> manyToOneForeignKeys)
        {
            var oneToManyType = prop.PropertyType;
            var oneToManySqlProperties = GetPropertiesList(oneToManyType);
            var oneToManyPrimaryKeys = SqlFieldInfo.GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return new SqlManyToOneReferenceInfo(prop, oneToManyType, oneToManyPrimaryKeys, manyToOneForeignKeys);
        }
    }
}
