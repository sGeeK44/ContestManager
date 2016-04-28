using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.DataStore.Sql.EntityInfo
{
    public class EntityInfoFactory : IEntityInfoFactory
    {
        public ISqlEntityInfo GetEntityInfo<T>()
        {
            return GetEntityInfo(typeof(T));
        }

        #region Entity class
             
        private SqlEntityInfo Create(Type classInfo, SqlEntityAttribute attr)
        {
            return new SqlEntityInfo(this, classInfo, attr);
        }

        public ISqlEntityInfo GetEntityInfo(Type classInfo)
        {
            if (classInfo == null) throw new ArgumentNullException("classInfo");

            var entityAttribute = classInfo.GetCustomAttributes(typeof(SqlEntityAttribute), true)
                                           .Cast<SqlEntityAttribute>()
                                           .FirstOrDefault();

            if (entityAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no SqlEntity attribute.", classInfo.Name));

            var entityInfo = Create(classInfo, entityAttribute);
            entityInfo.FieldList = GetSqlField(classInfo).Cast<ISqlPropertyInfo>().ToList();
            return entityInfo;
        }

        #endregion

        public static List<PropertyInfo> GetPropertiesList<T>()
        {
            return GetPropertiesList(typeof(T));
        }

        private static List<PropertyInfo> GetPropertiesList(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => item.IsDefined(typeof(SqlPropertyAttribute), true))
                    .Union(t.BaseType != null ? GetPropertiesList(t.BaseType).ToArray() : new PropertyInfo[0])
                    .ToList();
        }

        #region PrimaryKey field

        internal static List<SqlFieldInfo> GetPrimaryKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlPrimaryKeyAttribute))).Select(CreatePrimaryKey).ToList();
        }

        private static SqlFieldInfo CreatePrimaryKey(PropertyInfo prop)
        {
            var customAttr = prop.GetCustomAttributes(true);

            return new SqlPrimaryKeyFieldInfo(prop, customAttr);
        }

        #endregion

        #region ForeignKey field

        internal static List<SqlFieldInfo> GetForeignKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlForeignKeyAttribute))).Select(CreateForeignKey).ToList();
        }

        private static SqlFieldInfo CreateForeignKey(PropertyInfo prop)
        {
            var customAttr = prop.GetCustomAttributes(true);

            return new SqlForeignKeyFieldInfo(prop, customAttr);
        }

        #endregion

        #region Basic field


        public static List<SqlFieldInfo> GetSqlField<T>(object item = null)
        {
            return GetSqlField(typeof(T), item);
        }

        public static List<SqlFieldInfo> GetSqlField(Type type, object item = null)
        {
            var result = new List<SqlFieldInfo>();
            foreach (var prop in GetPropertiesList(type).Where(_ => _.IsDefined(typeof(SqlFieldAttribute))))
            {
                SqlFieldInfo field;
                if (prop.IsDefined(typeof(SqlPrimaryKeyAttribute))) field = CreatePrimaryKey(prop);
                else if (prop.IsDefined(typeof(SqlForeignKeyAttribute))) field = CreateForeignKey(prop);
                else field = Create(prop);
                result.Add(field);
            }
            return result;
        }

        public static SqlFieldInfo Create(string name, object[] customAttr)
        {
            return new SqlFieldInfo(null, customAttr);
        }

        private static SqlFieldInfo Create(PropertyInfo prop)
        {
            var customAttr = prop.GetCustomAttributes(true);

            return new SqlFieldInfo(prop, customAttr);
        }

        #endregion

        #region ManyToMany

        /// <summary>
        /// Get ManyToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToManyReferenceInfo of specified type</returns>
        public static List<SqlManyToManyReferenceInfo> GetManyToManySqlReference<T>()
        {
            var sqlProperties = GetPropertiesList<T>();
            var entityPrimaryKeys = GetPrimaryKeys(sqlProperties).Cast<ISqlPropertyInfo>().ToList();

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
            var referenceForeignKeys = GetPrimaryKeys(referenceSqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return new SqlManyToManyReferenceInfo(entityProperty, entityPrimaryKeys, referenceType, referenceForeignKeys);
        }

        private static bool IsAnalyseEntity<T>(Type type)
        {
            return type == typeof(T);
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

        #endregion

        #region ManyToOne


        /// <summary>
        /// Get ManyToOne reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToOneReferenceInfo of specified type</returns>
        public static List<SqlManyToOneReferenceInfo> GetManyToOneSqlReference<T>()
        {
            var manyToOneSqlProperties = GetPropertiesList<T>();
            var manyToOneForeignKeys = GetForeignKeys(manyToOneSqlProperties);
            return manyToOneSqlProperties.Where(_ => _.IsDefined(typeof(SqlManyToOneReferenceAttribute)))
                                          .Select(_ => Create(_, manyToOneForeignKeys))
                                          .ToList();
        }

        private static SqlManyToOneReferenceInfo Create(PropertyInfo prop, IEnumerable<ISqlPropertyInfo> manyToOneForeignKeys)
        {
            var oneToManyType = prop.PropertyType;
            var oneToManySqlProperties = GetPropertiesList(oneToManyType);
            var oneToManyPrimaryKeys = GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlPropertyInfo>().ToList();
            var foreignKeyForCurrentProperty = manyToOneForeignKeys.Where(_ => _.IsForeignKeyOf(prop)).ToList();
            return new SqlManyToOneReferenceInfo(prop, oneToManyType, oneToManyPrimaryKeys, foreignKeyForCurrentProperty);
        }

        #endregion

        #region OneToMany
        
        /// <summary>
        /// Get OneToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlOneToManyReferenceInfo of specified type</returns>
        public static List<SqlOneToManyReferenceInfo> GetOneToManySqlReference<T>()
        {
            var oneToManySqlProperties = GetPropertiesList<T>();
            var oneToManyPrimaryKeys = GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlPropertyInfo>().ToList();

            return oneToManySqlProperties.Where(_ => _.IsDefined(typeof(SqlOneToManyReferenceAttribute)))
                                         .Select(_ => Create(_, oneToManyPrimaryKeys))
                                         .ToList();
        }

        private static SqlOneToManyReferenceInfo Create(PropertyInfo oneToManyProperty, IList<ISqlPropertyInfo> oneToManyPrimaryKeys)
        {
            if (!IsList(oneToManyProperty.PropertyType)) throw new NotSupportedException(string.Format("Property type for OneToMany reference have to be an IList<>. Property:{0}.", oneToManyProperty.Name));

            var manyToOneType = oneToManyProperty.PropertyType.GetGenericArguments()[0];
            var manyToOneSqlProperties = GetPropertiesList(manyToOneType);
            var manyToOneForeignKeys = GetForeignKeys(manyToOneSqlProperties).Cast<ISqlPropertyInfo>().ToList();
            return new SqlOneToManyReferenceInfo(oneToManyProperty, manyToOneType, oneToManyPrimaryKeys, manyToOneForeignKeys);
        }

        #endregion
    }
}
