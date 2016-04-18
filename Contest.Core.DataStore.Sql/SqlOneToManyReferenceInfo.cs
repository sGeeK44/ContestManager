using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository;

namespace Contest.Core.DataStore.Sql
{
    public class SqlOneToManyReferenceInfo : SqlPropertyInfo
    {
        public List<SqlFieldInfo> PrimaryKeys { get; private set; }
        public Type ReferenceType { get; set; }

        private SqlOneToManyReferenceInfo(PropertyInfo prop)
            : base(prop)
        {
            if (!IsList(prop.PropertyType)) throw new NotSupportedException(string.Format("Property type for OneToMany reference have to be an IList<>. Property:{0}.", prop.Name));
            
            ReferenceType = prop.PropertyType.GetGenericArguments()[0];
        }

        public void FillReference(IUnitOfWorks unitOfWorks, object item)
        {
            var innerValue = unitOfWorks != null
                           ? unitOfWorks.Find(ReferenceType, GetPredicate(item))
                           : CreateEmptyListReference();

            SetValue(item, innerValue);
        }

        private IList CreateEmptyListReference()
        {
            var typeList = typeof(List<>).MakeGenericType(ReferenceType);
            return Activator.CreateInstance(typeList) as IList;
        }

        public LambdaExpression GetPredicate(object item)
        {
            // Manually build the expression tree for 
            // the lambda expression _ => _.ForeignKey == item.PrimaryKey.
            var foreignEntity = Expression.Parameter(ReferenceType, "_");
            var foreignKeyProp = Expression.Property(foreignEntity, "OneToManyEntityId");
            var primaryKeyProp = Expression.MakeMemberAccess(Expression.Constant(item), PrimaryKeys[0].PropertyInfo);

            var condition = Expression.Equal(foreignKeyProp, primaryKeyProp);
            var type = typeof(Func<,>).MakeGenericType(ReferenceType, typeof(bool));
            return Expression.Lambda(type, condition, new[] { foreignEntity });
        }

        public static List<SqlOneToManyReferenceInfo> GetSqlReference<T>()
        {
            var allSqlProp = GetPropertiesList<T>();
            var primaryKeys = SqlFieldInfo.GetPrimaryKeys(allSqlProp);
            return allSqlProp.Where(_ => _.IsDefined(typeof(SqlOneToManyReferenceAttribute)))
                                          .Select(_ => Create(_, primaryKeys))
                                          .ToList();
        }

        private static SqlOneToManyReferenceInfo Create(PropertyInfo prop, List<SqlFieldInfo> primaryKeys)
        {
            return new SqlOneToManyReferenceInfo(prop)
            {
                PrimaryKeys = primaryKeys
            };
        }

        private static bool IsList(Type t)
        {
            if (t == null) throw new NullReferenceException();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>);
        }
    }
}
