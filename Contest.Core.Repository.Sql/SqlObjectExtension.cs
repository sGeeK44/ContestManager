using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql
{
    public static class SqlObjectExtension
    {
        public static string ColumnName<TObj, TPropOrField>(this TObj obj, Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            return ColumnName(propertyorFieldExpression);
        }

        public static string ColumnName<TObj, TPropOrField>(Expression<Func<TObj, TPropOrField>> propertyorFieldExpression)
        {
            if (propertyorFieldExpression == null) throw new ArgumentNullException("propertyorFieldExpression");

            var body = propertyorFieldExpression.Body as MemberExpression;
            if (body == null) throw new ArgumentException("Invalid argument", "propertyorFieldExpression");
            
            var property = body.Member as PropertyInfo;
            var field = body.Member as FieldInfo;
            if (property == null && field == null)
                throw new ArgumentException("Argument is not a property or field", "propertyorFieldExpression");
            string name;
            DataMemberAttribute sqlAttribute;
            if (property != null)
            {
                name = property.Name;
                sqlAttribute = property.GetCustomAttributes(typeof(DataMemberAttribute), true)
                                       .Cast<DataMemberAttribute>()
                                       .FirstOrDefault();
            }
            else
            {
                name = field.Name;
                sqlAttribute = field.GetCustomAttributes(typeof(DataMemberAttribute), true)
                                    .Cast<DataMemberAttribute>()
                                    .FirstOrDefault();
            }
            if (sqlAttribute == null) throw new NotSupportedException("Properties or field doesn't contains DataMemberAttribute.");

            return sqlAttribute.Name ?? name;
        }
    }
}
