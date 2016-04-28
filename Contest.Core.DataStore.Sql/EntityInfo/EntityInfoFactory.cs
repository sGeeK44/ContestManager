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

        public ISqlEntityInfo GetEntityInfo(Type classInfo)
        {
            if (classInfo == null) throw new ArgumentNullException("classInfo");

            var entityAttribute = classInfo.GetCustomAttributes(typeof(SqlEntityAttribute), true)
                                           .Cast<SqlEntityAttribute>()
                                           .FirstOrDefault();

            if (entityAttribute == null) throw new NotSupportedException(string.Format("Class {0} has no SqlEntity attribute.", classInfo.Name));

            var entityInfo = Create(classInfo, entityAttribute);
            entityInfo.FieldList = GetSqlField(classInfo).ToList();
            entityInfo.ReferenceList = GetSqlReference(classInfo).ToList();
            return entityInfo;
        }
             
        private SqlEntityInfo Create(Type classInfo, SqlEntityAttribute attr)
        {
            return new SqlEntityInfo(this, classInfo, attr);
        }

        private static List<PropertyInfo> GetPropertiesList(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(item => item.IsDefined(typeof(SqlPropertyAttribute), true))
                    .Union(t.BaseType != null ? GetPropertiesList(t.BaseType).ToArray() : new PropertyInfo[0])
                    .ToList();
        }

        private static IEnumerable<SqlFieldInfo> GetPrimaryKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlPrimaryKeyAttribute))).Select(CreatePrimaryKey).ToList();
        }

        private static SqlFieldInfo CreatePrimaryKey(PropertyInfo prop)
        {
            return new SqlPrimaryKeyFieldInfo(prop);
        }

        private static IEnumerable<SqlFieldInfo> GetForeignKeys(IEnumerable<PropertyInfo> allSqlProp)
        {
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlForeignKeyAttribute))).Select(CreateForeignKey).ToList();
        }

        private static SqlFieldInfo CreateForeignKey(PropertyInfo prop)
        {

            return new SqlForeignKeyFieldInfo(prop);
        }

        public static IEnumerable<ISqlFieldInfo> GetSqlField(Type type)
        {
            var result = new List<ISqlFieldInfo>();
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

        private static IEnumerable<ISqlReferenceInfo> GetSqlReference(Type type)
        {
            var result = GetManyToOneSqlReference(type);
            result.AddRange(GetOneToManySqlReference(type));
            result.AddRange(GetManyToManySqlReference(type));
            return result;
        }

        private static SqlFieldInfo Create(PropertyInfo prop)
        {
            return new SqlFieldInfo(prop);
        }

        /// <summary>
        /// Get ManyToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToManyReferenceInfo of specified type</returns>
        public static List<ISqlReferenceInfo> GetManyToManySqlReference<T>()
        {
            return GetManyToManySqlReference(typeof (T));
        }

        private static List<ISqlReferenceInfo> GetManyToManySqlReference(Type type)
        {
            var sqlProperties = GetPropertiesList(type);
            var entityPrimaryKeys = GetPrimaryKeys(sqlProperties).Cast<ISqlFieldInfo>().ToList();

            return sqlProperties.Where(_ => _.IsDefined(typeof(SqlManyToManyReferenceAttribute)))
                                             .Select(_ => Create(type, _, entityPrimaryKeys))
                                             .ToList();
        }

        private static ISqlReferenceInfo Create(Type type, PropertyInfo entityProperty, IList<ISqlFieldInfo> entityPrimaryKeys)
        {
            var propertyType = entityProperty.PropertyType;
            if (!IsList(propertyType)) throw new NotSupportedException(string.Format("Property type for ManyToMany reference have to be an IList<>. Property:{0}.", entityProperty.Name));

            var referenceType = propertyType.GenericTypeArguments[0];
            if (!IsRelationShip(referenceType)) throw new NotSupportedException(string.Format("Reference for ManyToMany relation have to be an Relation<,>. Property:{0}.", entityProperty.Name));

            var firstRelationType = referenceType.GenericTypeArguments[0];

            var referenceSqlProperties = GetPropertiesList(referenceType).Where(_ => _.Name == (IsAnalyseEntity(type, firstRelationType) ? "FirstItemInvolveId" : "SecondItemInvolveId"));
            var referenceForeignKeys = GetPrimaryKeys(referenceSqlProperties).Cast<ISqlFieldInfo>().ToList();
            return new SqlManyToManyReferenceInfo(entityProperty, entityPrimaryKeys, referenceType, referenceForeignKeys);
        }

        /// <summary>
        /// Get ManyToOne reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlManyToOneReferenceInfo of specified type</returns>
        public static List<ISqlReferenceInfo> GetManyToOneSqlReference<T>()
        {
            return GetManyToOneSqlReference(typeof (T));
        }

        /// <summary>
        /// Get ManyToOne reference of specified type
        /// </summary>
        /// <returns>All SqlManyToOneReferenceInfo of specified type</returns>
        private static List<ISqlReferenceInfo> GetManyToOneSqlReference(Type type)
        {
            var manyToOneSqlProperties = GetPropertiesList(type);
            var manyToOneForeignKeys = GetForeignKeys(manyToOneSqlProperties);
            return manyToOneSqlProperties.Where(_ => _.IsDefined(typeof(SqlManyToOneReferenceAttribute)))
                                         .Select(_ => Create(_, manyToOneForeignKeys))
                                         .ToList();
        }

        private static ISqlReferenceInfo Create(PropertyInfo prop, IEnumerable<ISqlFieldInfo> manyToOneForeignKeys)
        {
            var oneToManyType = prop.PropertyType;
            var oneToManySqlProperties = GetPropertiesList(oneToManyType);
            var oneToManyPrimaryKeys = GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlFieldInfo>().ToList();
            var foreignKeyForCurrentProperty = manyToOneForeignKeys.Where(_ => _.IsForeignKeyOf(prop)).ToList();
            return new SqlManyToOneReferenceInfo(prop, oneToManyType, oneToManyPrimaryKeys, foreignKeyForCurrentProperty);
        }
        
        /// <summary>
        /// Get OneToMany reference of specified type
        /// </summary>
        /// <typeparam name="T">Type to analyse</typeparam>
        /// <returns>All SqlOneToManyReferenceInfo of specified type</returns>
        public static List<ISqlReferenceInfo> GetOneToManySqlReference<T>()
        {
            return GetOneToManySqlReference(typeof (T));
        }

        private static List<ISqlReferenceInfo> GetOneToManySqlReference(Type type)
        {
            var oneToManySqlProperties = GetPropertiesList(type);
            var oneToManyPrimaryKeys = GetPrimaryKeys(oneToManySqlProperties).Cast<ISqlFieldInfo>().ToList();

            return oneToManySqlProperties.Where(_ => _.IsDefined(typeof(SqlOneToManyReferenceAttribute)))
                                         .Select(_ => Create(_, oneToManyPrimaryKeys))
                                         .ToList();
        }

        private static ISqlReferenceInfo Create(PropertyInfo oneToManyProperty, IList<ISqlFieldInfo> oneToManyPrimaryKeys)
        {
            if (!IsList(oneToManyProperty.PropertyType)) throw new NotSupportedException(string.Format("Property type for OneToMany reference have to be an IList<>. Property:{0}.", oneToManyProperty.Name));

            var manyToOneType = oneToManyProperty.PropertyType.GetGenericArguments()[0];
            var manyToOneSqlProperties = GetPropertiesList(manyToOneType);
            var manyToOneForeignKeys = GetForeignKeys(manyToOneSqlProperties).Cast<ISqlFieldInfo>().ToList();
            return new SqlOneToManyReferenceInfo(oneToManyProperty, manyToOneType, oneToManyPrimaryKeys, manyToOneForeignKeys);
        }

        private static bool IsAnalyseEntity(Type type, Type relationType)
        {
            return type == relationType;
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
